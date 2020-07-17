using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace TeamBuilderPkmnASP.Models
{
    public class User
    {
        [NotMapped]
        public static User DefaultUser
        {
            get
            {
                return new User { Mail = "", Password = "" };
            }
        }
        [Key]
        [Column("mail")]
        public string Mail { get; set; }
        [Column("pseudo")]
        public string Pseudo { get; set; }
        [DefaultValue(true)]
        [NotMapped]
        public bool IsPwdInputHashed { get; set; }
        [Column("password")]
        public string Password
        {
            get
            {
                return hashedPwd;
            }

            set
            {
                if (!IsPwdInputHashed)
                {
                    using (var sha = new SHA256Managed())
                    {
                        byte[] array = Encoding.Default.GetBytes(value);
                        array = sha.ComputeHash(array);
                        StringBuilder sb = new StringBuilder();
                        foreach (var b in array)
                        {
                            sb.Append(b.ToString("x2"));
                        }
                        hashedPwd = sb.ToString();
                    }
                }
                else
                {
                    hashedPwd = value;
                }
            }
        }
        [Column("verification_token")]
        public string VerificationToken { get; set; }
        [Column("is_verified")]
        public bool IsVerified { get; set; }
        
        [NotMapped]
        public List<Team> Teams { get; set; }

        private string hashedPwd;

        public User()
        {
            IsPwdInputHashed = true;
        }
    }
}
