using AcAp = Autodesk.AutoCAD.ApplicationServices;
using AcEd = Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(LayoutRenamer.AutoCAD.Commands))]

namespace LayoutRenamer.AutoCAD
{
    public class Commands
    {
        [CommandMethod("UpdateLayouts")]
        public void UpdateLayouts()
        {
            AcAp.Document doc = AcAp.Application.DocumentManager.MdiActiveDocument;
            AcEd.Editor ed = doc.Editor;
            ed.WriteMessage("\nHello from LayoutRenamer (.NET 8)!");

            // TODO: Call Core methods
        }
    }
}