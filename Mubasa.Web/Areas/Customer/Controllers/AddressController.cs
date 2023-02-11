using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility.ThirdParties.Carrier;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace Mubasa.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AddressController : Controller
    {
        private readonly IUnitOfWork _db;
        private IOptions<GiaoHangNhanh> _ghn;

        public AddressController(IUnitOfWork db, IOptions<GiaoHangNhanh> ghn)
        {
            _db = db;
            _ghn = ghn;
        }

        // GET: AddressController
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim == null)
            {
                return NotFound();
            }

            var addresses = _db.Address.GetAll(
                i => i.ApplicationUserId == claim.Value, 
                includeProp: "Ward,District,Province");

            var user = _db.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value);
            if (user.AddressId == null)
            {
                user.AddressId = addresses.First().Id;
                _db.Save();
            }

            var defaultAddress = addresses.FirstOrDefault(i => i.Id == user.AddressId);
            if(defaultAddress != null)
            {
                defaultAddress.IsDefault = true;
            }

            return View(addresses);
        }

        // GET: AddressController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddressController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Address address)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (claim == null)
                {
                    return NotFound();
                }

                var province = _db.Province.GetFirstOrDefault(
                    i => i.Name == address.Province.Name);

                var district = _db.District.GetFirstOrDefault(
                    i => i.Name == address.District.Name &&
                        i.ProvinceId == province.Id);

                var ward = _db.Ward.GetFirstOrDefault(
                    i => i.Name == address.Ward.Name &&
                        i.DistrictId == district.Id);

                if (province.Id != 0 &&
                    district.Id != 0 &&
                    ward.Id != 0)
                {
                    address.ProvinceId = province.Id;
                    address.Province = null;
                    address.DistrictId = district.Id;
                    address.District = null;
                    address.WardId = ward.Id;
                    address.Ward = null;
                    address.ApplicationUserId = claim.Value;

                    _db.Address.Add(address);
                    _db.Save();

                    TempData["success"] = "Thêm địa chỉ thành công";

                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: AddressController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddressController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var address = _db.Address.GetFirstOrDefault(i => i.Id == id);

                if (address == null)
                {
                    return Json(new { success = false, message = $"Not Found" });
                }

                _db.Address.Remove(address);
                _db.Save();

                return Json(new { success = true, message = $"Delete Successful" });
            }
            catch
            {
                return Json(new { success = false, message = $"Error Deleting Data" });
            }
        }

        [HttpGet]
        public async Task<IActionResult>LoadAddress(string type, int? id)
        {
            try
            {
                GiaoHangNhanh GHN = new GiaoHangNhanh()
                {
                    EndPoint = _ghn.Value.EndPoint,
                    Token = _ghn.Value.Token
                };

                IEnumerable<dynamic>? addressData;

                switch (type)
                {
                    case "province":
                        addressData = await GHN.GetAddress($"province");
                        break;
                    case "district":
                        addressData = await GHN.GetAddress($"district?province_id={id}");
                        break;
                    case "ward":
                        addressData = await GHN.GetAddress($"ward?district_id={id}");
                        break;
                    default:
                        return Json(new { success = false, data = "Not Found" });
                }

                return Json(new { success = true, data = JsonObject.Parse(addressData.ToJson()) });
            }
            catch (Exception)
            {
                return Json(new { success = false, data = $"Something went wrong" });
            }
        }
    }
}
