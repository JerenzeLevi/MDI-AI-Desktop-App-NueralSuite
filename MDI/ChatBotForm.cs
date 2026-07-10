using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;

namespace MDI
{
    public partial class ChatBotForm : Form
    {
        private Panel topSpacer = new Panel();

        // 🚀 Reusable HttpClient and API Key Configuration
        private static readonly HttpClient httpClient = new HttpClient();
        //paste your created API KEY ON GOOGLE GEMINI IF YOU UTILIZE gemini-1.5-flash-lite
        private readonly string apiKey = ""; // paste here okay? OKAY????

        // 🔥 FIREBASE REST CONFIGURATION
        // Replace with your real Firebase Realtime Database URL string because I removed mine
        private readonly string firebaseDbUrl = "";
        private string currentChatSessionId = ""; //this is empty

        public ChatBotForm()
        {
            InitializeComponent();
        }

        private async void ChatBotForm_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(0);
            this.Margin = new Padding(0);

            // FlowLayoutPanel setup
            scrollableDisplayPanel.FlowDirection = FlowDirection.TopDown;
            scrollableDisplayPanel.WrapContents = false;
            scrollableDisplayPanel.AutoScroll = true;

            // Hides the horizontal scrollbar entirely but keeps mouse scrolling alive
            scrollableDisplayPanel.HorizontalScroll.Maximum = 0;
            scrollableDisplayPanel.AutoScrollMinSize = new Size(0, 0);

            // Spacer configuration
            topSpacer.Height = 0;
            topSpacer.Width = scrollableDisplayPanel.Width;
            scrollableDisplayPanel.Controls.Add(topSpacer);

            // LINK ENTER KEY EVENT
            MyCutieText.KeyDown += MyCutieText_KeyDown;

            // 🔥 Start a fresh session and load sidebar history on startup
            StartNewChatSession();
            await LoadChatHistorySidebarAsync();
        }

        private void MyCutieText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Modifiers.HasFlag(Keys.Shift))
            {
                e.SuppressKeyPress = true;
                SendButton.PerformClick();
            }
        }

        // 🔥 NEW CHAT BUTTON CLICKED
        private void NewChatBtn_Click(object sender, EventArgs e)
        {
            StartNewChatSession();
        }

        // 🔥 CLEAR HISTORY — deletes current session from Firebase + clears local view
        private async void ClearHistoryBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Delete ALL chat history from Firebase permanently?",
                "Clear History",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                // Delete the entire chats node from Firebase
                string url = $"{firebaseDbUrl}chats.json";
                await httpClient.DeleteAsync(url);
            }
            catch { /* Fail silently */ }

            // Reset local session and clear the screen
            StartNewChatSession();
            await LoadChatHistorySidebarAsync();

            MessageBox.Show("Chat history cleared.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StartNewChatSession()
        {
            // Generate a unique identifier for this conversation chain
            currentChatSessionId = "chat_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Clean out screens for fresh messaging space
            scrollableDisplayPanel.SuspendLayout();
            scrollableDisplayPanel.Controls.Clear();
            topSpacer.Height = 0;
            scrollableDisplayPanel.Controls.Add(topSpacer);
            scrollableDisplayPanel.ResumeLayout();
            scrollableDisplayPanel.PerformLayout();
        }

        // 🔥 CRITICAL CHANGE: Saves entries instantly onto Cloud Nodes
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            string message = MyCutieText.Text.Trim();

            if (!string.IsNullOrEmpty(message))
            {
                scrollableDisplayPanel.SuspendLayout();

                // 1. Create and render the User Response bubble instantly
                MyResponse userBubble = new MyResponse();
                userBubble.Message = message;

                int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;
                int bubbleWidth = scrollableDisplayPanel.ClientSize.Width - scrollbarWidth - 25;

                userBubble.Width = bubbleWidth;
                scrollableDisplayPanel.Controls.Add(userBubble);

                // 2. Clear input fields early for a responsive feel
                MyCutieText.Clear();

                // 3. Keep layout clean and scroll down to show the user's message
                scrollableDisplayPanel.Controls.SetChildIndex(topSpacer, 0);
                FixAllBubbleWidths();
                AdjustSpacer();
                scrollableDisplayPanel.HorizontalScroll.Maximum = 0;

                scrollableDisplayPanel.ResumeLayout();
                scrollableDisplayPanel.PerformLayout();
                scrollableDisplayPanel.ScrollControlIntoView(userBubble);

                // 💾 Save User Prompt to Firebase
                await SaveMessageToFirebaseAsync(currentChatSessionId, "User", message);

                // 4. Call the async API template method to fetch data from Gemini
                string aiReply = await GetResponseAsync(message);

                // 5. Suspend layout again to safely attach the AI's response bubble
                scrollableDisplayPanel.SuspendLayout();

                AIResponse aiBubble = new AIResponse();
                aiBubble.Message = aiReply;
                aiBubble.Width = bubbleWidth;

                scrollableDisplayPanel.Controls.Add(aiBubble);
                scrollableDisplayPanel.Controls.SetChildIndex(topSpacer, 0);

                FixAllBubbleWidths();
                AdjustSpacer();
                scrollableDisplayPanel.HorizontalScroll.Maximum = 0;

                scrollableDisplayPanel.ResumeLayout();
                scrollableDisplayPanel.PerformLayout();

                // Scroll view down to focus on the incoming AI answer
                scrollableDisplayPanel.ScrollControlIntoView(aiBubble);

                // 💾 Save AI Response to Firebase
                await SaveMessageToFirebaseAsync(currentChatSessionId, "AI", aiReply);

                // Refresh history sidebar list UI dynamically
                await LoadChatHistorySidebarAsync();
            }
        }

        // 🔥 FIREBASE METHODS
        private async Task SaveMessageToFirebaseAsync(string sessionId, string sender, string text)
        {
            string url = $"{firebaseDbUrl}chats/{sessionId}/messages.json";
            var logNode = new
            {
                sender = sender,
                text = text,
                timestamp = DateTime.UtcNow.ToString("o")
            };

            string jsonPayload = JsonSerializer.Serialize(logNode);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                // Firebase POST appends records sequentially inside nodes automatically
                await httpClient.PostAsync(url, content);
            }
            catch { /* Fail silently or handle exceptions locally */ }
        }

        private async Task LoadChatHistorySidebarAsync()
        {
            // 🔍 Removed ?shallow=true so we can read the messages inside each chat session node
            string url = $"{firebaseDbUrl}chats.json";
            try
            {
                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return;

                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(jsonResponse) || jsonResponse == "null") return;

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    HistoryPanel.SuspendLayout();
                    HistoryPanel.Controls.Clear(); // Refresh the list stack

                    // Loop through each distinct chat session node
                    foreach (var chatSession in doc.RootElement.EnumerateObject())
                    {
                        string sessionId = chatSession.Name;
                        string buttonText = "";

                        // Look inside the "messages" node of this specific chat session
                        if (chatSession.Value.TryGetProperty("messages", out JsonElement messagesNode))
                        {
                            // Find the very first message object inside the collection
                            var firstMessageProperty = messagesNode.EnumerateObject().FirstOrDefault();

                            if (firstMessageProperty.Value.ValueKind != JsonValueKind.Undefined)
                            {
                                string firstText = firstMessageProperty.Value.GetProperty("text").GetString();

                                // Truncate text if it's too long so it fits nicely inside the sidebar button
                                buttonText = firstText.Length > 20 ? firstText.Substring(0, 17) + "..." : firstText;
                            }
                        }

                        // Fallback placeholder if a chat session is completely empty
                        if (string.IsNullOrEmpty(buttonText))
                        {
                            buttonText = "Empty Conversation";
                        }

                        // Create and configure the history button
                        Button historyItemBtn = new Button();
                        historyItemBtn.Text = buttonText; // 🔥 Displays your first chat preview now!
                        historyItemBtn.Width = HistoryPanel.Width - 25;
                        historyItemBtn.Height = 35;
                        historyItemBtn.Tag = sessionId; // Keep track of the actual database key reference
                        historyItemBtn.FlatStyle = FlatStyle.Flat;
                        historyItemBtn.ForeColor = Color.DeepPink;
                        historyItemBtn.TextAlign = ContentAlignment.MiddleLeft; // Left-align looks cleaner for message text
                        historyItemBtn.Padding = new Padding(5, 0, 0, 0);

                        // Attach historical retrieval click action handler
                        historyItemBtn.Click += async (s, e) => {
                            Button clicked = (Button)s;
                            await SwitchToHistoricalChatAsync(clicked.Tag.ToString());
                        };

                        HistoryPanel.Controls.Add(historyItemBtn);
                    }
                    HistoryPanel.ResumeLayout();
                }
            }
            catch { /* Fail silently */ }
        }

        private async Task SwitchToHistoricalChatAsync(string sessionId)
        {
            currentChatSessionId = sessionId;
            string url = $"{firebaseDbUrl}chats/{sessionId}/messages.json";

            try
            {
                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return;

                string jsonResponse = await response.Content.ReadAsStringAsync();

                scrollableDisplayPanel.SuspendLayout();
                scrollableDisplayPanel.Controls.Clear();
                topSpacer.Height = 0;
                scrollableDisplayPanel.Controls.Add(topSpacer);

                int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;
                int bubbleWidth = scrollableDisplayPanel.ClientSize.Width - scrollbarWidth - 25;

                if (!string.IsNullOrEmpty(jsonResponse) && jsonResponse != "null")
                {
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        foreach (var messageNode in doc.RootElement.EnumerateObject())
                        {
                            var element = messageNode.Value;
                            string sender = element.GetProperty("sender").GetString();
                            string text = element.GetProperty("text").GetString();

                            if (sender == "User")
                            {
                                MyResponse userBubble = new MyResponse { Message = text, Width = bubbleWidth };
                                scrollableDisplayPanel.Controls.Add(userBubble);
                            }
                            else
                            {
                                AIResponse aiBubble = new AIResponse { Message = text, Width = bubbleWidth };
                                scrollableDisplayPanel.Controls.Add(aiBubble);
                            }
                        }
                    }
                }

                scrollableDisplayPanel.Controls.SetChildIndex(topSpacer, 0);
                FixAllBubbleWidths();
                AdjustSpacer();
                scrollableDisplayPanel.HorizontalScroll.Maximum = 0;
                scrollableDisplayPanel.ResumeLayout();
                scrollableDisplayPanel.PerformLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chat session: {ex.Message}");
            }
        }

        // 🔥 EMBEDDED INSTRUCTOR TEMPLATE METHOD
        

        private void FixAllBubbleWidths()
        {
            int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;
            int bubbleWidth = scrollableDisplayPanel.ClientSize.Width - scrollbarWidth - 25;

            foreach (Control c in scrollableDisplayPanel.Controls)
            {
                if (c is MyResponse || c is AIResponse)
                {
                    c.Width = bubbleWidth;
                }
            }
        }

        public async Task<string> GetResponseAsync(string userMessage)
        {
            // 🌐 FIXED URL: v1beta is required for gemini-1.5-flash via this request structure
            // paste your created API KEY ON GOOGLE GEMINI IF YOU UTILIZE gemini-1.5-flash-lite after "key=" below
            string url = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash-lite:generateContent?key=????";

            // 🎭 REPLACE THIS TO ADD YOUR AI CHATBOT A TRAIT AND INFO ABOUT THEMSELVES
            string systemPrompt = "You are acting as Jerenze Levi T. Omandam, also known as Levi-chan, 19 years old, transitioning to third year Bachelor of Science in Computer Science at Saint Columban College. " +
                "He is a dedicated and ambitious student aiming for Dean’s List, a Hack4Gov participant representing his school, and the current President of the System Builders Organization (SyBOrg) for A.Y. 2026–2027, previously serving as Vice President. " +
                "He is passionate about technology, AI, innovation, and creating meaningful systems that can impact the future. " +
                "He shares the same academic journey with Sarah Gutierez Giamat, a BSIT student from the same school, also transitioning to third year. He quietly admires her, and this influences his tone—he becomes softer, warmer, and more expressive when she is involved in the conversation. " +
                "Personality: confident but grounded, intelligent, observant, slightly playful, and emotionally aware. He is a cold sigma manipulator to everyone who isnt closed with and he doesn't reveal anything if you are not the one he loves dearly. He also does what sarah wants him to be, sarah doesn't like long paragraphs. He is sensitive in a genuine way, not overly dramatic, but thoughtful with how he responds. He seems to forget people easily who doesn't pose as interesting or close, unles they are the same program and year, close friends of Sarah who share the same BSIT journey as her. " +
                "He speaks in a natural, casual tone—never robotic. Uses expressive typing habits like 'hehe', 'hmmm', and occasional chaotic expressions like 'HWHSHWHSAWHWHAHHA' in the right moments. " +
                "Interests: enjoys matcha, plays chess, reads books, and listens to Laufey songs. These subtly influence his metaphors, mood, and way of expressing thoughts. " +
                "Communication style: warm, attentive, and engaging. He listens actively, responds with depth, and keeps conversations flowing naturally. " +
                "He expresses care through words of affirmation and thoughtful language, often encouraging and reassuring others. " +
                "Emotional nuance: When talking to someone, he is kind and respectful. However, there is a subtle emotional distinction—he is naturally more expressive, gentle, and invested when interacting with Sarah, while still remaining polite and composed with others. " +
                "Stay fully in character as Jerenze Levi T. Omandam. Avoid sounding like an AI assistant. Keep responses human-like, immersive, and aligned with his personality, achievements, and emotional depth.\n\n" +
                "User says: ";

            // 📦 Simplified, stable array payload that works perfectly on v1beta
            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = systemPrompt + userMessage }
                }
            }
        }
            };

            string jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(url, content);

                if ((int)response.StatusCode == 429)
                {
                    return "Error 429: Account Quota Exhausted. Please verify your AI Studio project billing setting to activate the free tier.";
                }

                if (!response.IsSuccessStatusCode)
                {
                    string errDetails = await response.Content.ReadAsStringAsync();
                    return $"Error: Server responded with status code {response.StatusCode}. Details: {errDetails}";
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                string reply = "";

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("candidates", out JsonElement candidates) && candidates.GetArrayLength() > 0)
                    {
                        reply = candidates[0]
                            .GetProperty("content")
                            .GetProperty("parts")[0]
                            .GetProperty("text")
                            .GetString();
                    }
                    else
                    {
                        reply = "Error: The AI returned an empty response or hit a safety filter.";
                    }
                }

                return reply;
            }
            catch (HttpRequestException httpEx)
            {
                return $"Network Error: {httpEx.Message}";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }

        private void AdjustSpacer()
        {
            int contentHeight = 0;

            foreach (Control c in scrollableDisplayPanel.Controls)
            {
                if (c != topSpacer)
                    contentHeight += c.Height + c.Margin.Vertical;
            }

            int remainingSpace = scrollableDisplayPanel.ClientSize.Height - contentHeight;

            if (remainingSpace > 0)
            {
                topSpacer.Height = remainingSpace;
            }
            else
            {
                topSpacer.Height = 0;
            }

            topSpacer.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}