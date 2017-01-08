using UnityEngine;
using System.Collections;

namespace Twitter {

	public class Warning {

		private string code { get; set; }
		private string message { get; set; }
		private int percent_full { get; set; }

		public Warning () {}

		public Warning (string code, string message, int percent_full){
		
			this.code = code;
			this.message = message;
			this.percent_full = percent_full;
		
		}

	}

}