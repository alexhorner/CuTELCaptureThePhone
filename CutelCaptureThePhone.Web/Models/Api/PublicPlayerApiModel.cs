using CutelCaptureThePhone.Core.Models;

namespace CutelCaptureThePhone.Web.Models.Api
{
    public class PublicPlayerApiModel
    {
        public string Name { get; set; } = null!;
        public string RegisteredFromNumber { get; set; } = null!;

        public static PublicPlayerApiModel FromModel(PlayerModel model) => new()
        {
            Name = model.Name,
            RegisteredFromNumber = model.RegisteredFromNumber
        };
    }
}