using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Domain
{
	public static class Globals
	{
		public static bool IsWorkMachine
		{
			get
			{
				return Environment.MachineName.StartsWith("BOU");
			}
		}

		public static string LocalPath
		{
			get
			{
				if (IsWorkMachine)
				{
					return @"C:\Projects\Playground\SportsStore\";
				}

				return @"C:\Users\Harris\Documents\Visual Studio 2012\Projects\ProAspMvc4\SportsStore\";
			}
		}
	}
}