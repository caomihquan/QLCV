using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLCV.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "Tên Đăng Nhập/Email")]
        [Required(ErrorMessage = "Bạn Phải Nhập Tài Khoản hoặc Email")]
        public string UserName { set; get; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Bạn Phải Nhập Mật Khẩu")]
        public string Password { set; get; }
        public bool RememberMe { set; get; }
        public string JavascriptToRun { get; set; }
    }
}