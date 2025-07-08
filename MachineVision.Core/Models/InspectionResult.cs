using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.Models
{
    public class InspectionResult:BindableBase
    {
		private bool isSuccess;
        private double timeSpan;

        public bool IsSuccess
        {
			get { return isSuccess; }
			set { isSuccess = value;RaisePropertyChanged(); }
		}

		public double TimeSpan
        {
			get { return timeSpan; }
			set { timeSpan = value; RaisePropertyChanged(); }
		}

		private string resString;

		public string ResString
        {
			get { return resString; }
			set { resString = value;RaisePropertyChanged(); }
		}

		private int sn;

		public int SN
		{
			get { return sn; }
			set { sn = value; RaisePropertyChanged(); }
		}


	}
}
