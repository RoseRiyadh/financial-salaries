using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZulfieP.Models
{
    public class AuthInfo
    {
	
			public AuthInfo()
			{
				
			}
			public int UserId { get; set; }
			public string UserName { get; set; }

			public bool IsAuthenticated => (UserId > 0);
	
	}
}
