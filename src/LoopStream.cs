using NAudio.Wave;

namespace Konsolka;

/// <summary>
/// Stream for looping playback
/// Sources:
/// https://github.com/naudio/NAudio/issues/473
/// https://www.markheath.net/post/looped-playback-in-net-with-naudio
/// </summary>
public class LoopStream : WaveStream
{
    readonly WaveStream _sourceStream;

    /// <summary>
    /// Creates a new Loop stream
    /// </summary>
    /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
    /// or else we will not loop to the start again.</param>
    public LoopStream(WaveStream sourceStream)
    {
        _sourceStream = sourceStream;
        EnableLooping = true;
    }

    /// <summary>
    /// Use this to turn looping on or off
    /// </summary>
    private bool EnableLooping { get; }

    /// <summary>
    /// Return source stream's wave format
    /// </summary>
    public override WaveFormat WaveFormat => _sourceStream.WaveFormat;

    /// <summary>
    /// LoopStream simply returns
    /// </summary>
    public override long Length => _sourceStream.Length;

    /// <summary>
    /// LoopStream simply passes on positioning to source stream
    /// </summary>
    public override long Position
    {
        get => _sourceStream.Position;
        set => _sourceStream.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int totalBytesRead = 0;

        while (totalBytesRead < count)
        {
            int bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
            if (bytesRead == 0)
            {
                if (_sourceStream.Position == 0 || !EnableLooping)
                {
                    throw new FileLoadException("File is corrupted");
                }
                
                _sourceStream.Position = 0;
            }
            totalBytesRead += bytesRead;
        }
        return totalBytesRead;
    }
}