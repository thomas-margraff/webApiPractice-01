using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.Dto
{
	public class ReleaseDto
	{
		public int Id { get; set; }
		public string Currency { get; set; }
		public string Indicator { get; set; }
		public List<IndicatorDataDto> IndicatorDataDto { get; set; }

		public ReleaseDto()
		{
			this.IndicatorDataDto = new List<IndicatorDataDto>();
		}
	}

	public class IndicatorDataDto
	{
		public int IndicatorId { get; set; }
		public int EventId { get; set; }
		public string ReleaseDate { get; set; }
		public DateTime ReleaseDateTime { get; set; }
		public string ReleaseTime { get; set; }
		public string Actual { get; set; }
		public string Forecast { get; set; }
		public string Previous { get; set; }
	}

}
