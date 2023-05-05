using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Drawing;

namespace FindPartInDrawings
{
    public class Program
    {
        static void Main(string[] args)
        {
            ViewSearcher();
            Console.WriteLine("===== results should be the same =======");
            SheetSearcher();
        }

        private static void ViewSearcher()
        {
            ModelObjectEnumerator selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();
            if(selectedObjects.GetSize() == 0)
            {
                Console.WriteLine("no object selected");
                return;
            }
    
            DrawingHandler drawingHandler = new DrawingHandler();
            if (drawingHandler.GetConnectionStatus())
            {
                DrawingEnumerator selectedDrawings = drawingHandler.GetDrawingSelector().GetSelected();
                if (selectedDrawings.GetSize() == 0)
                {
                    Console.WriteLine("no drawing selected");
                    return;
                }

                while (selectedDrawings.MoveNext())
                {
                    Drawing currentDrawing = selectedDrawings.Current;
                    DrawingObjectEnumerator drawingViews = currentDrawing.GetSheet().GetViews();
                    while (drawingViews.MoveNext())
                    {
                        View view = drawingViews.Current as View;
                        while (selectedObjects.MoveNext())
                        {
                            if (selectedObjects.Current is Tekla.Structures.Model.Part part)
                            {
                                if (view.GetModelObjects(part.Identifier).GetSize() > 0)
                                {
                                    Console.WriteLine($"{part.Identifier.GUID} was found in " +
                                        $"drawing: {currentDrawing.Name}, " +
                                        $"view: {view.Name}");
                                }
                                else
                                {
                                    Console.WriteLine($"{part.Identifier.GUID} not found");
                                }
                            }
                        }
                        selectedObjects.Reset();
                    }
                }
                Console.WriteLine("done");
            }
            else
            {
                Console.WriteLine("No connection");
            }
        }

        private static void SheetSearcher()
        {
            ModelObjectEnumerator selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();
            if (selectedObjects.GetSize() == 0)
            {
                Console.WriteLine("no object selected");
                return;
            }

            DrawingHandler drawingHandler = new DrawingHandler();
            if (drawingHandler.GetConnectionStatus())
            {
                DrawingEnumerator selectedDrawings = drawingHandler.GetDrawingSelector().GetSelected();
                if (selectedDrawings.GetSize() == 0)
                {
                    Console.WriteLine("no drawing selected");
                    return;
                }

                while (selectedDrawings.MoveNext())
                {
                    Drawing currentDrawing = selectedDrawings.Current;
                    ContainerView sheet = currentDrawing.GetSheet();
                    while (selectedObjects.MoveNext())
                    {
                        if (selectedObjects.Current is Tekla.Structures.Model.Part part)
                        {
                            if (sheet.GetModelObjects(part.Identifier).GetSize() > 0)
                            {
                                Console.WriteLine($"{part.Identifier.GUID} was found in " +
                                    $"drawing: {currentDrawing.Name}");
                            }
                            else
                            {
                                Console.WriteLine($"{part.Identifier.GUID} not found");
                            }
                        }
                    }
                    selectedObjects.Reset();
                }
                Console.WriteLine("done");
            }
            else
            {
                Console.WriteLine("No connection");
            }
        }

    }
}
