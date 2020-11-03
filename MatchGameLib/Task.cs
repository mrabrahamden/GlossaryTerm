using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


//namespace MatchGameLib
//{
//        internal partial class TaskForm : Form
//        {
//            private readonly TaskSettings _settings;

//            public RelationTaskForm(TaskSettings settings)
//            {
//                InitializeComponent();

//                _settings = settings;

//                PrepareForm();
//            }


//            private void PrepareForm()
//            {
//                lTaskDescription.Text = _settings.TaskDescription;

//                var rnd = new Random((int)DateTime.Now.Ticks);
//                pAllWords.Controls.AddRange(_settings.Phrases.Values.OrderBy(s => rnd.Next(_settings.Phrases.Count)).Select(w => GetLabelForWord(w)).ToArray());

//                pPhrases.RowCount = _settings.Phrases.Count + 1;
//                pPhrases.RowStyles.Clear();
//                for (int i = 0; i < pPhrases.RowCount; i++)
//                {
//                    pPhrases.RowStyles.Add(new RowStyle(SizeType.Percent, (float)(100 / (pPhrases.RowCount + 1))));
//                }

//                int rowIndex = 1;
//                foreach (string phrase in _settings.Phrases.Keys)
//                {
//                    Label phraseLabel = new Label
//                    {
//                        Font = new Font("Arial", 12, FontStyle.Bold),
//                        Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
//                        Text = phrase
//                    };
//                    pPhrases.Controls.Add(phraseLabel, 0, rowIndex);

//                    Panel answerPanel = new FlowLayoutPanel
//                    {
//                        Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
//                        AllowDrop = true,
//                        BackColor = Color.LightBlue
//                    };
//                    pPhrases.Controls.Add(answerPanel, 1, rowIndex);

//                    answerPanel.DragEnter += new DragEventHandler(answerPanel_DragEnter);
//                    answerPanel.DragDrop += new DragEventHandler(answerPanel_DragDrop);

//                    rowIndex++;
//                }
//            }

//            void answerPanel_DragDrop(object sender, DragEventArgs e)
//            {
//                Panel panel = (Panel)sender;
//                if (panel.Controls.Count > 0)
//                    return;

//                var label = (Label)e.Data.GetData(typeof(Label).FullName);
//                label.Parent.Controls.Remove(label);

//                panel.Controls.Add(label);
//                panel.Invalidate();
//            }

//            void answerPanel_DragEnter(object sender, DragEventArgs e)
//            {
//                var panel = (Panel)sender;

//                if (e.Data.GetDataPresent(typeof(Label).FullName) &&
//                    panel.Controls.Count == 0)
//                {
//                    e.Effect = DragDropEffects.Move;
//                }
//            }

//            private Label GetLabelForWord(string w)
//            {
//                var label = new Label
//                {
//                    Text = w,
//                    AutoSize = true,
//                    BorderStyle = BorderStyle.FixedSingle,
//                    BackColor = Color.LightSkyBlue,
//                    Padding = new Padding(3),
//                };

//                label.MouseDown += new MouseEventHandler(label_MouseDown);

//                return label;
//            }

//            void label_MouseDown(object sender, MouseEventArgs e)
//            {
//                var label = (Label)sender;

//                label.DoDragDrop(label, DragDropEffects.Move);
//            }

//            private void OnCheckResult(object sender, EventArgs e)
//            {
//                var answers = new Dictionary<string, string>();
//                for (int i = 1; i < pPhrases.RowCount; i++)
//                {
//                    var phrase = ((Label)pPhrases.GetControlFromPosition(0, i)).Text;

//                    var answerPanel = (Panel)pPhrases.GetControlFromPosition(1, i);

//                    var answerLabel = answerPanel.Controls.OfType<Label>().FirstOrDefault();

//                    var meaning = answerLabel == null ? string.Empty : answerLabel.Text;

//                    answers[phrase] = meaning;
//                }

//                new RelationTaskResultForm(_settings, answers).ShowDialog();
//                Close();
//            }

//        }
    

//}
