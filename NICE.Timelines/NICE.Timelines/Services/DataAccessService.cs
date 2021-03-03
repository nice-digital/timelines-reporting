using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NICE.Timelines.Models;

namespace NICE.Timelines.Services
{
	public interface IDataAccessService
	{
		void SaveTask(Models.TimelinesTask clickUpTask);
	}


	public class DataAccessService : IDataAccessService
	{
		public void SaveTask(Models.TimelinesTask clickUpTask)
		{
			throw new NotImplementedException();
		}
	}
}
