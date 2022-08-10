using HtmlAgilityPack;
using mshtml;
using SpiceLogic.HtmlEditorControl.Domain.BOs;
using SpiceLogic.WpfHtmlEditor.EditorEventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZabgcTool_SDK_.HtmlRedactor
{
    /// <summary>
    /// Логика взаимодействия для Redactor.xaml
    /// </summary>
    public partial class EditRedactor : Window
    {
        public string Text;
        public EditRedactor()
        {
            InitializeComponent();
            
            Edit.Click += async (s, e) =>
            {
            

                Saver.Text = await SiteSaver(WpfHtmlEditor.Content.GetBodyHtml(true));
                Close();
            };
            SaveName.Click += (s, e) => { Textbox.Visibility = Visibility.Collapsed; };
        }
        public EditRedactor(string text, Action Save)
        {

            MouseLeftButtonDown += (s, e) => { DragMove(); };
            Text = text;
            InitializeComponent();

            WpfHtmlEditor.Content.InsertHtml(ParseHtml(Text), false);
            //NameData.Click += (s, e) =>
            //{
            //    Textbox.Visibility = Visibility.Visible;
            //};
            
            Edit.Click += async (s, e) =>
            {
                Saver.Text = SaveParseHtml(await SiteSaver(WpfHtmlEditor.Content.GetBodyHtml(true)));
                Save();
                await Task.Delay(3000);
                Close();

            };
            SaveName.Click += (s, e) => { Textbox.Visibility = Visibility.Collapsed; };
        }
        public EditRedactor(string text, int a)
        {
             
            Text = text;
            InitializeComponent();

            WpfHtmlEditor.Content.InsertHtml(ParseHtml(Text), false);
            //NameData.Click += (s, e) =>
            //{
            //    Textbox.Visibility = Visibility.Visible;
            //};
            Edit.Click += async (s, e) =>
            {
                Saver.Text = SaveParseHtml(await SiteSaver(WpfHtmlEditor.Content.GetBodyHtml(true)));
                
                Close(); 
                
            };
            SaveName.Click += (s, e) => { Textbox.Visibility = Visibility.Collapsed; };
        }

        private string SaveParseHtml(string Html) {
            string html = Html;
            List<string> imgScrs = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);//or doc.Load(htmlFileStream)
            var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");
            if (nodes != null)
            {
                foreach (var img in nodes)
                {
                    HtmlAttribute att = img.Attributes["src"];
                    imgScrs.Add(att.Value);
                    var src = @"\";
                    int ind = html.IndexOf($"src=\"{att.Value}\"");
                    string data = $"src={src}\"{att.Value}\"";
                    if (att.Value.Contains("http://zabgc.ru"))
                    {
                        var path = att.Value.Split('/');
                        string newpath = string.Empty;
                        for(int i = 3; i <= path.Length -1; i++)
                        {
                            newpath += '/'+path[i];
                        }
                      //  System.Windows.Forms.MessageBox.Show(newpath);
                        html = html.Remove(ind, data.Length).Insert(ind, $"src=\"{newpath}\" ");
                    }
                }
            }

            return html;
        }
        private string ParseHtml(string Html)
        {
            string html = Html;
            List<string> imgScrs = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);//or doc.Load(htmlFileStream)
            var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");
            if (nodes != null)
            {
                foreach (var img in nodes)
                {
                    HtmlAttribute att = img.Attributes["src"];
                    imgScrs.Add(att.Value);
                    var src = @"\";
                    int ind = html.IndexOf($"src=\"{att.Value}\"");
                    string data = $"src={src}\"{att.Value}\"";
                    if (!att.Value.Contains("http://zabgc.ru"))
                    {
                        html = html.Remove(ind, data.Length).Insert(ind, $"src=\"http://zabgc.ru{att.Value}\" ");
                    }
                }
            }

            return html;
        }
        public async Task<string> SaveAsync()
        {
            return await  SiteSaver(WpfHtmlEditor.Content.GetBodyHtml(true));
        }
        private async Task<string> SiteSaver(string Html)
        {
            string html = Html;
            var settings = new Helper.Settings().ReadSettings();
            FTP.Client client = new FTP.Client(settings.Addres,settings.Login, settings.Password);
            List<string> imgScrs = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);//or doc.Load(htmlFileStream)
            var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");
            if (nodes != null)
            {
                foreach (var img in nodes)
                {
                    HtmlAttribute att = img.Attributes["src"];
                    imgScrs.Add(att.Value);
                    var src = @"\";
                    int ind = html.IndexOf($"src=\"{att.Value}\"");
                    string data = $"src={src}\"{att.Value}\"";
                    if (!att.Value.Contains("http://zabgc.ru"))
                    {
                        string PathFile = "file/" + Environment.TickCount.ToString() +$"-{DateTime.Now.ToShortDateString()}-"+ Path.GetExtension(att.Value);
                  await  client.UploadFile(att.Value, PathFile);
                    
                        html = html.Remove(ind, data.Length).Insert(ind, $"src=\"{PathFile}\" ");
                    }
                }
            }

            return html;
        }
        private void contextMenuItem_imageProperties_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.ToolbarItemOverrider.OnImageButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles Click event of the LinkProperties context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_linkProperties_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.ToolbarItemOverrider.OnHyperLinkButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles Click event of the CellProperties context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_cellProperties_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.ToolbarItemOverrider.OnTableCellEditingClicked(sender, e);
        }

        /// <summary>
        /// Handles Click event of the TableProperties context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void tablePropertiesToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.ToolbarItemOverrider.OnTableModifyButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles Click event of the InsertRowBefore context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void insertRowBeforeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Content.TableAuthoringService.InsertRow(InsertPositions.Before);
        }

        /// <summary>
        /// Handles Click event of the InsertRowAfter context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void insertRowAfterToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Content.TableAuthoringService.InsertRow(InsertPositions.After);
        }

        /// <summary>
        /// Handles Click event of the DeleteRow context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void deleteRowToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Content.TableAuthoringService.DeleteRow();
        }

        /// <summary>
        /// Handles Click event of the InsertColumnBefore context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void insertColumnBeforeToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Content.TableAuthoringService.InsertColumn(InsertPositions.Before);
        }

        /// <summary>
        /// Handles Click event of the InsertColumnAfter context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void insertColumnAfterToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Content.TableAuthoringService.InsertColumn(InsertPositions.After);
        }

        /// <summary>
        /// Handles Click event of the DeleteColumn context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void deleteColumnToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Content.TableAuthoringService.DeleteColumn();
        }

        /// <summary>
        /// Handles Click event of the MergeCells context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void mergeCellsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Content.TableAuthoringService.MergeSelectedCells();
        }

        /// <summary>
        /// Handles Click event of the YouTubeVideoProperties context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void youTubeVideoPropertiesToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.ToolbarItemOverrider.OnYouTubeVideoInsertButtonClicked(sender, e);
        }

        /// <summary>
        /// Handles Click event of the AlignLeft context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_align_left_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Formatting.AlignLeft();
        }

        /// <summary>
        /// Handles Click event of the AlignCenter context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_align_center_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Formatting.AlignCenter();
        }

        /// <summary>
        /// Handles Click event of the AlignRight context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_align_right_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Formatting.AlignRight();
        }

        /// <summary>
        /// Handles Click event of the AlignRemove context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_align_remove_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Formatting.RemoveAlignment();
        }

        /// <summary>
        /// Handles Click event of the Cut context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_cut_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Editor.Cut();
        }

        /// <summary>
        /// Handles Click event of the Copy context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_copy_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Editor.Copy();
        }

        /// <summary>
        /// Handles Click event of the Paste context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_paste_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Editor.Paste();
        }

        /// <summary>
        /// Handles Click event of the Delete context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_delete_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Editor.Delete();
        }

        /// <summary>
        /// Handles Click event of the SelectAll context menu item
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void contextMenuItem_selectAll_Click(object sender, RoutedEventArgs e)
        {
            WpfHtmlEditor.Selection.SelectAll();
        }

        /// <summary>
        /// Handles ContextMenuShowing event of the WpfHtmlEditor
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void WpfHtmlEditor_OnContextMenuShowing(object sender, ContextMenuShowingEventArgs e)
        {
            ContextMenu customContextMenu = FindResource("ContextMenuCustom") as ContextMenu;
            if (customContextMenu == null)
                return;

            MenuItem contextMenuItemDelete = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_delete") as MenuItem;
            if (contextMenuItemDelete != null)
                contextMenuItemDelete.IsEnabled = WpfHtmlEditor.StateQuery.CanDelete();

            MenuItem contextMenuItemCopy = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_copy") as MenuItem;
            if (contextMenuItemCopy != null)
                contextMenuItemCopy.IsEnabled = WpfHtmlEditor.StateQuery.CanCopy();

            MenuItem contextMenuItemCut = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_cut") as MenuItem;
            if (contextMenuItemCut != null)
                contextMenuItemCut.IsEnabled = WpfHtmlEditor.StateQuery.CanCut();

            MenuItem contextMenuItemPaste = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_paste") as MenuItem;
            if (contextMenuItemPaste != null)
                contextMenuItemPaste.IsEnabled = WpfHtmlEditor.StateQuery.CanPaste();

            MenuItem youTubeVideoPropertiesToolStripMenuItem = LogicalTreeHelper.FindLogicalNode(customContextMenu, "youTubeVideoPropertiesToolStripMenuItem") as MenuItem;
            if (youTubeVideoPropertiesToolStripMenuItem != null)
                youTubeVideoPropertiesToolStripMenuItem.Visibility = WpfHtmlEditor.StateQuery.IsYouTubeVideo() ? Visibility.Visible : Visibility.Collapsed;

            MenuItem contextMenuItemImageProperties = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_imageProperties") as MenuItem;
            MenuItem contextMenuItemLinkProperties = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_linkProperties") as MenuItem;

            IHTMLElement activeHtmlElement = WpfHtmlEditor.StateQuery.GetActiveHtmlElement();
            if (WpfHtmlEditor.StateQuery.IsImage()
                && activeHtmlElement.parentElement.tagName.ToLower() == "a"
                && activeHtmlElement.parentElement.innerHTML == activeHtmlElement.outerHTML)
            {
                //"Captured : Image link"
                if (contextMenuItemLinkProperties != null && contextMenuItemLinkProperties.IsVisible)
                    contextMenuItemLinkProperties.IsChecked = true;

                if (contextMenuItemImageProperties != null && contextMenuItemImageProperties.IsVisible)
                    contextMenuItemImageProperties.IsChecked = true;
            }
            else
            {
                if (contextMenuItemLinkProperties != null && contextMenuItemLinkProperties.IsVisible)
                    contextMenuItemLinkProperties.IsChecked = WpfHtmlEditor.StateQuery.IsActiveOrAncestorElementHyperLink();

                if (contextMenuItemImageProperties != null && contextMenuItemImageProperties.IsVisible)
                    contextMenuItemImageProperties.IsChecked = WpfHtmlEditor.StateQuery.IsImage();
            }

            Separator toolStripSeparator1Context = LogicalTreeHelper.FindLogicalNode(customContextMenu, "toolStripSeparator1Context") as Separator;
            if (toolStripSeparator1Context != null)
                toolStripSeparator1Context.Visibility = (WpfHtmlEditor.StateQuery.IsTable() || WpfHtmlEditor.StateQuery.IsTableCell() || WpfHtmlEditor.StateQuery.IsHyperLink()) ? Visibility.Visible : Visibility.Collapsed;

            MenuItem contextMenuItemTableProperties = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_tableProperties") as MenuItem;
            if (contextMenuItemTableProperties != null)
                contextMenuItemTableProperties.Visibility = WpfHtmlEditor.StateQuery.IsTable() || WpfHtmlEditor.StateQuery.IsTableCell() ? Visibility.Visible : Visibility.Collapsed;

            MenuItem contextMenuItemCellProperties = LogicalTreeHelper.FindLogicalNode(customContextMenu, "contextMenuItem_cellProperties") as MenuItem;
            if (contextMenuItemCellProperties != null)
                contextMenuItemCellProperties.Visibility = WpfHtmlEditor.StateQuery.IsTableCell() ? Visibility.Visible : Visibility.Collapsed;

            MenuItem mergeCellsToolStripMenuItem = LogicalTreeHelper.FindLogicalNode(customContextMenu, "mergeCellsToolStripMenuItem") as MenuItem;
            if (mergeCellsToolStripMenuItem != null)
                mergeCellsToolStripMenuItem.IsEnabled = WpfHtmlEditor.StateQuery.CanMergeTableCells();
        }

        /// <summary>
        /// Handles Loaded event of the MainWindow
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ContextMenu customContextMenu = FindResource("ContextMenuCustom") as ContextMenu;
            if (customContextMenu == null)
                return;

            WpfHtmlEditor.EditorContextMenuStrip = customContextMenu;
        }

        private void butns_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


    }
}

