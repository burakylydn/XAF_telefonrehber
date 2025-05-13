//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Text;
//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.DC;
//using DevExpress.ExpressApp.Model;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.BaseImpl;
//using DevExpress.Persistent.Validation;

//namespace telefonrehber.Api.Controllers
//{
    
    
//    public class ValidationController : ControllerBase
//    {
//        private readonly IEmailValidationService _emailValidationService;
//        private readonly IPhoneValidationService _phoneValidationService;

//        public ValidationController(IEmailValidationService emailValidationService, IPhoneValidationService phoneValidationService)
//        {
//            _emailValidationService = emailValidationService;
//            _phoneValidationService = phoneValidationService;
//        }

//        [HttpPost("validate-email")]
//        public async Task<IActionResult> ValidateEmail([FromBody] string email)
//        {
//            bool isValid = await _emailValidationService.ValidateEmailAsync(email);
//            if (isValid)
//                return Ok("Geçerli bir e-posta.");
//            else
//                return BadRequest("E-posta geçersiz veya mevcut deðil.");
//        }

//        [HttpPost("validate-phone")]
//        public async Task<IActionResult> ValidatePhone([FromBody] string phoneNumber)
//        {
//            bool isValid = await _phoneValidationService.ValidatePhoneAsync(phoneNumber);
//            if (isValid)
//                return Ok("Geçerli bir telefon numarasý.");
//            else
//                return BadRequest("Telefon numarasý geçersiz veya mevcut deðil.");
//        }
//    }
//}