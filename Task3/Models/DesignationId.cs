using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task3.Models
{
	public class DesignationId
	{
		[Required]
        public int Id { get; set; }
		[Required]
        public string Identifier { get; set; }

    }
}