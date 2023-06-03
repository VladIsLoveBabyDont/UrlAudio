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
            string tempFile = Path.GetTempFileName(); // Создаем временный файл

            // Загружаем аудиофайл во временный файл
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    using (var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // Копируем содержимое ответа в файл
                        await response.Content.CopyToAsync(fileStream);
                    }
                }
            }

            // Воспроизводим аудиофайл
            using (var audioOutput = new WaveOutEvent())
            using (var audioFile = new AudioFileReader(tempFile))
            {
                audioOutput.Init(audioFile);
                audioOutput.Play();

                // В данном примере мы будем держать поток открытым до тех пор, пока звук не закончится.
                while (audioOutput.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(1000);
                }
            }

            File.Delete(tempFile); // Удаляем временный файл
        }
    }
}