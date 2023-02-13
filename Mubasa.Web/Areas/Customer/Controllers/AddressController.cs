using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Web.Services.ThirdParties.Carrier.GiaoHangNhanh;
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

                address.ApplicationUserId = claim.Value;

                _db.Address.Add(address);
                _db.Save();

                TempData["success"] = "Thêm địa chỉ thành công";

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // GET: AddressController/Edit/5
        public ActionResult Edit(int id)
        {
            var address = _db.Address.GetFirstOrDefault(i => i.Id == id, 
                includeProp: "Ward,District,Province");

            return View(address);
        }

        // POST: AddressController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Address address)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                
                address.ApplicationUserId = claim.Value;

                _db.Address.Update(address);
                _db.Save();

                TempData["success"] = "Cập nhật thành công";
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
        public IActionResult GetProvince()
        {
            try
            {
                var province = _db.Province.GetAll();

                return Json(new { success = true, data = JsonObject.Parse(province.ToJson()) });
            }
            catch (Exception)
            {
                return Json(new { success = false, data = $"Something went wrong" });
            }
        }

        [HttpGet]
        public IActionResult GetDistrict(int province_id)
        {
            try
            {
                var district = _db.District.GetAll(i => i.ProvinceId == province_id);

                return Json(new { success = true, data = JsonObject.Parse(district.ToJson()) });
            }
            catch (Exception)
            {
                return Json(new { success = false, data = $"Something went wrong" });
            }
        }

        [HttpGet]
        public IActionResult GetWard(int district_id)
        {
            try
            {
                var ward = _db.Ward.GetAll(i => i.DistrictId == district_id);

                return Json(new { success = true, data = JsonObject.Parse(ward.ToJson()) });
            }
            catch (Exception)
            {
                return Json(new { success = false, data = $"Something went wrong" });
            }
        }
        
        public IActionResult SetDefault(int address_id)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var user = _db.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value);
                user.AddressId = address_id;
                _db.Save();

                TempData["success"] = "Đã thêm vào địa chỉ mặc định";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View();
            }
            
        }
        
    }
}
