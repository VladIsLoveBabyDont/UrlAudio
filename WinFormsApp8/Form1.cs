namespace WinFormsApp8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayAudioFromUrl();
        }

        public async Task PlayAudioFromUrl(string url)
        {
            string tempFile = Path.GetTempFileName(); // ������� ��������� ����

            // ��������� ��������� �� ��������� ����
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    using (var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // �������� ���������� ������ � ����
                        await response.Content.CopyToAsync(fileStream);
                    }
                }
            }

            // ������������� ���������
            using (var audioOutput = new WaveOutEvent())
            using (var audioFile = new AudioFileReader(tempFile))
            {
                audioOutput.Init(audioFile);
                audioOutput.Play();

                // � ������ ������� �� ����� ������� ����� �������� �� ��� ���, ���� ���� �� ����������.
                while (audioOutput.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(1000);
                }
            }

            File.Delete(tempFile); // ������� ��������� ����
        }
    }
}