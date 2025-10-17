using System.ComponentModel;
using System.Reflection.Metadata;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(LayoutRenamer.AutoCAD.Commands))]

namespace LayoutRenamer.AutoCAD
{
    public class Commands
    {
        [CommandMethod("UpdateLayouts")]
        public void UpdateLayouts()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            EditorAttribute ed = doc.editor;
            ed.WriteMessage("\nHello from LayoutRenamer (.NET 8)!");

            // TODO: Call Core methods
        }
    }
}