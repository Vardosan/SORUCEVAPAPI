using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.ViewModel
{
    public class KayitModel
    {
        public string kayitId { get; set; }
        public string kayitSoruId { get; set; }
        public string kayitDersId { get; set; }
        public string kayitCevapId { get; set; }
        public SoruModel soruBilgi { get; set; }
        public DersModel dersBilgi { get; set; }
        public CevapModel cevapBilgi { get; set; }
    }
}