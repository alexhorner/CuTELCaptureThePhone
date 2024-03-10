using CutelPhoneGame.Core.Models;

namespace CutelPhoneGame.Web.Models.Api
{
    public class ApiPlayerModel
    {
        public uint Pin { get; set; }
        public string RegisteredFromNumber { get; set; } = null!;

        public static ApiPlayerModel FromModel(PlayerModel model) => new()
        {
            Pin = model.Pin,
            RegisteredFromNumber = model.RegisteredFromNumber
        };
    }
}