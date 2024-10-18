using System;
using System.Collections.Generic;
using System.Drawing;
using SharpShell.SharpThumbnailHandler;
using System.IO;
using SharpShell.Attributes;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Text;
using OMRON_IFZ_Viewer;

namespace IfzThumbnailHandler
{
    /// <summary>
    /// The IfzThumbnailHandler is a ThumbnailHandler for IFZ OMRON files
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.FileExtension, ".ifz")]
    public class IfzThumbnailHandler : SharpThumbnailHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IfzThumbnailHandler"/> class
        /// </summary>
        /// 

        public IfzThumbnailHandler()
        {
            //  Create our lazy objects
            lazyThumbnailFont = new Lazy<Font>(() => new Font("Courier New", 12f));
            lazyThumbnailTextBrush = new Lazy<Brush>(() => new SolidBrush(Color.Black));
        }

        /// <summary>
        /// Gets the thumbnail image
        /// </summary>
        /// <param name="width">The width of the image that should be returned.</param>
        /// <returns>
        /// The image for the thumbnail
        /// </returns>
        protected override Bitmap GetThumbnailImage(uint width)
        {
           
            //  Create a stream reader for the selected item stream
            try
            {


                using (var reader = new StreamReader(SelectedItemStream))
                {
                    Bitmap mybitmap;
                    FiltLibIF.BayerMaster bayerMaster = new FiltLibIF.BayerMaster();
                    FiltLibIF.MakeByrDataFromStream(reader.BaseStream, ref bayerMaster);
                    FiltLibIF.ByrtoBmp(bayerMaster, out mybitmap, 0);

                    return CreateThumbnailForImage(mybitmap, width);
                }
                ////  Read up to ten lines of text
                //var previewLines = new List<string>();
                //previewLines.Add("coucou");
                ////  Now return a preview of the lines
                //return CreateThumbnailForText(previewLines, width);

            }
            catch (Exception exception)
            {
                //  Log the exception and return null for failure
                LogError("An exception occurred opening the text file.", exception);
                return null;
            }
        }

        /// <summary>
        /// Creates the thumbnail for image, using the provided bitmap
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>
        /// A thumbnail for the image
        /// </returns>
        private Bitmap CreateThumbnailForImage(Bitmap image, uint widthin)
        {
            int w = (int)widthin;
            int h = w * 2 / 3;
            Bitmap bitmap = new Bitmap(w, h);
            Graphics graphic = Graphics.FromImage(bitmap);
            graphic.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);
            float width = (float)w / (float)image.Width;
            float height = (float)h / (float)image.Height;
            float single = Math.Min(width, height);
            width = (float)image.Width * single;
            height = (float)image.Height * single;
            graphic.DrawImage(image, ((float)w - width) / 2f, ((float)h - height) / 2f, width, height);
            graphic.Dispose();

            //  Return the bitmap
            return bitmap;
        }

        /// <summary>
        /// Creates the thumbnail for text, using the provided preview lines
        /// </summary>
        /// <param name="previewLines">The preview lines.</param>
        /// <param name="width">The width.</param>
        /// <returns>
        /// A thumbnail for the text
        /// </returns>
        private Bitmap CreateThumbnailForText(IEnumerable<string> previewLines, uint width)
        {
            //  Create the bitmap dimensions
            var thumbnailSize = new Size((int)width, (int)width);

            //  Create the bitmap
            var bitmap = new Bitmap(thumbnailSize.Width,
                                    thumbnailSize.Height, PixelFormat.Format32bppArgb);

            //  Create a graphics object to render to the bitmap
            using (var graphics = Graphics.FromImage(bitmap))
            {
                //  Set the rendering up for anti-aliasing
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                //  Draw the page background
                graphics.DrawImage(Properties.Resources.Page, 0, 0,
                                   thumbnailSize.Width, thumbnailSize.Height);

                //  Create offsets for the text
                var xOffset = width * 0.2f;
                var yOffset = width * 0.3f;
                var yLimit = width - yOffset;

                graphics.Clip = new Region(new RectangleF(xOffset, yOffset,
                  thumbnailSize.Width - (xOffset * 2), thumbnailSize.Height - width * .1f));

                //  Render each line of text
                foreach (var line in previewLines)
                {
                    graphics.DrawString(line, lazyThumbnailFont.Value,
                                        lazyThumbnailTextBrush.Value, xOffset, yOffset);
                    yOffset += 14f;
                    if (yOffset + 14f > yLimit)
                        break;
                }
            }

            //  Return the bitmap
            return bitmap;
        }

        /// <summary>
        /// The lazy thumbnail font
        /// </summary>
        private readonly Lazy<Font> lazyThumbnailFont;

        /// <summary>
        /// The lazy thumbnail text brush
        /// </summary>
        private readonly Lazy<Brush> lazyThumbnailTextBrush;
    }

 
}
