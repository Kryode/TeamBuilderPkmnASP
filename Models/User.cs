using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using KryodeHelpers;

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
                    hashedPwd = ConnectionHelper.SHA256StringConstructor(value);
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
