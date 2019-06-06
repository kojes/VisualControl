namespace VueExample.Color
{
    public interface IColorService
    {
        string GetRandomHexColor();
        string GetHexColorByDieId (long? die);

    }
}