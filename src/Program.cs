using Konsolka;
using NAudio.Wave;

public class Program
{
    public static async Task Main()
    {
        var mp3Reader = new Mp3FileReader(new FileStream(@"./Sounds/chipi-chipi-chapa-chapa.mp3", FileMode.Open));
        LoopStream loop = new LoopStream(mp3Reader);
        using var waveOut = new WaveOutEvent();
        waveOut.Init(loop);
        waveOut.Play();
        
        while (true)
        {
            foreach (var catFace in CatFaces.Array)
            {
                Console.WriteLine(catFace);
                await Task.Delay(500);
                Console.Clear();
            }
        }
    }
}