using LayoutRenamer.Core.Config;
using LayoutRenamer.AutoCAD.Services;
using Autodesk.AutoCAD.Runtime;
using AcAp = Autodesk.AutoCAD.ApplicationServices;
using AcEd = Autodesk.AutoCAD.EditorInput;

[assembly: CommandClass(typeof(LayoutRenamer.AutoCAD.Commands))]

namespace LayoutRenamer.AutoCAD
{
    public class Commands
    {
        [CommandMethod("ReadTBTitle")]
        public void ReadTitleBlockTitle()
        {
            // 1) Grab editor for feedback
            AcAp.Document doc = AcAp.Application.DocumentManager.MdiActiveDocument;
            AcEd.Editor   ed  = doc.Editor;

            try
            {
                // 2) Build config file path (repo-relative example)
                //    Adjust path if you place config elsewhere.
                string repoRoot = System.IO.Path.GetFullPath(
                    System.IO.Path.Combine(doc.Name, @"..\..\..") // heuristic: DWG may not be in repo; fallback below
                );

                string fallback = "C:\\Users\\tommy\\Documents\\AutoCAD\\autocad-update-layout-name-csharp"; // optional

                string cfgPath  = System.IO.Path.Combine(repoRoot, "config", "blockAttributes.json");
                if (!System.IO.File.Exists(cfgPath))
                {
                    // Fallback to a known absolute path if needed (edit this to your machine)
                    cfgPath = System.IO.Path.Combine(fallback, "config", "blockAttributes.json");
                }

                // 3) Load config
                TitleBlockConfig cfg = ConfigLoader.Load(cfgPath);

                // 4) Use the service to read attributes on the current layout
                var svc = new AutoCADLayoutService();
                var (sheet, title) = svc.ReadTitleFromCurrentLayout(cfg);

                // 5) Report findings in the command line
                ed.WriteMessage($"\nSheet: {(string.IsNullOrWhiteSpace(sheet) ? "(none)" : sheet)}");
                ed.WriteMessage($"\nTitle: {(string.IsNullOrWhiteSpace(title) ? "(none)" : title)}\n");
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"\n[Error] {ex.Message}\n");
            }
        }
    }
}
