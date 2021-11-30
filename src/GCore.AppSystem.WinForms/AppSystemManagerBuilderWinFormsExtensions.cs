namespace GCore.AppSystem.WinForms;

public static class AppSystemManagerBuilderWinFormsExtensions
{
    public static void AddWinForms(this IAppSystemManagerBuilder self)
    {
        self.AddScannableAssemblies(typeof(AppSystemManagerBuilderWinFormsExtensions).Assembly);
    }
}