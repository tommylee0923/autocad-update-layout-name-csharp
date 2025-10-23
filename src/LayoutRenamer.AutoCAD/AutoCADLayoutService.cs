using System.Text;
using LayoutRenamer.Core.Config;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using AcAp = Autodesk.AutoCAD.ApplicationServices;
using AcDb = Autodesk.AutoCAD.DatabaseServices;
using AcEd = Autodesk.AutoCAD.EditorInput;

namespace LayoutRenamer.AutoCAD.Services
{
    public sealed class AutoCADLayoutService
    {
        /// <summary>
        /// Reads the sheet number and multi-line title from the configured title block
        /// that exists on the CURRENT layout's BlockTableRecord. Returns (sheet, title).
        /// </summary>
        public (string? SheetNumber, string TitleString) ReadTitleFromCurrentLayout(TitleBlockConfig cfg)
        {
            // 1) Get doc/db/editor
            AcAp.Document doc = AcAp.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            AcEd.Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {

                // 2) Resolve the current layout's BlockTableRecord (paperspace of current tab)
                LayoutManager lm = LayoutManager.Current;
                ObjectId layId = lm.GetLayoutId(lm.CurrentLayout);
                Layout layout = (Layout)tr.GetObject(layId, OpenMode.ForRead);
                ObjectId btrId = layout.BlockTableRecordId;
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(btrId, OpenMode.ForRead);

                // 3) Scan ALL entities on this layout for a block reference with our name
                //    (works even if there are viewports/other inserts on the sheet)
                BlockReference? targetBr = null;
                foreach (ObjectId entId in btr)
                {
                    if (!entId.ObjectClass.IsDerivedFrom(RXClass.GetClass(typeof(BlockReference))))
                        continue;

                    var br = (BlockReference)tr.GetObject(entId, OpenMode.ForRead);

                    string effectiveName = GetEffectiveBlockName(br, tr);

                    // Compare case-insensitively to TitleBlockName from config
                    if (string.Equals(effectiveName, cfg.TitleBlockName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetBr = br;
                        break; // stop at first match
                    }
                }

                if (targetBr is null)
                    return (null, "");

                // 4) Build a tag→text lookup from the block's attributes
                var attTexts = ReadAttributes(targetBr, tr);

                // 5) Extract sheet number
                attTexts.TryGetValue(cfg.SheetNum.ToUpperInvariant(), out string? sheet);

                // 6) Extract title lines IN THE ORDER specified by cfg.Title
                var sb = new StringBuilder();
                foreach (var tag in cfg.Title)
                {
                    if (attTexts.TryGetValue(tag.ToUpperInvariant(), out string? val) && !string.IsNullOrWhiteSpace(val))
                    {
                        if (sb.Length > 0) sb.Append(' ');
                        sb.Append(val.Trim());
                    }
                }

                tr.Commit();
                return (sheet, sb.ToString().Trim());
            }
        }

        /// <summary>
        /// For dynamic blocks, returns the effective block name (not the anonymous name).
        /// For static blocks, returns br.Name.
        /// </summary>
        private static string GetEffectiveBlockName(BlockReference br, Transaction tr)
        {
            if (br.IsDynamicBlock)
            {
                var dynBtr = (AcDb.BlockTableRecord)tr.GetObject(br.DynamicBlockTableRecord, OpenMode.ForRead);
                return dynBtr.Name;
            }
            return br.Name;
        }

        /// <summary>
        /// Reads attribute TAG→TEXT from a BlockReference. Keys are upper-cased.
        /// </summary>
        private static Dictionary<string, string> ReadAttributes(BlockReference br, Transaction tr)
        {
            var dict = new Dictionary<string, string>();
            if (br.AttributeCollection != null && br.AttributeCollection.Count > 0)
            {
                foreach (ObjectId attId in br.AttributeCollection)
                {
                    if (!attId.IsValid) continue;
                    var attRef = tr.GetObject(attId, OpenMode.ForRead) as AttributeReference;
                    if (attRef is null) continue;

                    string tag = (attRef.Tag ?? "").Trim().ToUpperInvariant();
                    string text = (attRef.TextString ?? "").Trim();

                    if (!string.IsNullOrEmpty(tag))
                        dict[tag] = text;
                }
            }
            return dict;
        }
    }
}
