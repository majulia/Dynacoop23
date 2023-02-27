using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.VO
{
    public class ProductVO
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }
    }
}
