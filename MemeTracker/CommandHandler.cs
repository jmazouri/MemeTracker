using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using MemeTracker.StoragePocos;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.Compression;
using NAudio.Wave.SampleProviders;

namespace MemeTracker
{
    public class CommandHandler
    {
        static IDiscordVoiceClient _voiceClient;

        public static async Task<string> HandleCommand(DiscordClient client, DiscordMessage msg)
        {
            return await Task.Run(async delegate
            {
                if (msg.Message.StartsWith("!t1"))
                {
                    var foundChannel = msg.OriginalMessage.Server.VoiceChannels.First(d => d.Members.Any(v => v.Name == "jmazouri"));

                    if (_voiceClient == null)
                    {
                        await client.JoinVoiceServer(foundChannel);
                        _voiceClient = client.GetVoiceClient(msg.OriginalMessage.Server);
                    }
                    else
                    {
                        await _voiceClient.JoinChannel(foundChannel.Id);
                    }

                    var mp3Read = new Mp3FileReader("meme.mp3");

                    var resampler = new MediaFoundationResampler(mp3Read, new WaveFormat(48000, 16, 1));

                    byte[] buffer = new byte[32000];

                    while (resampler.Read(buffer, 0, buffer.Length) > 0)
                    {
                        _voiceClient.SendVoicePCM(buffer, buffer.Length);
                    }

                    mp3Read.Dispose();
                    resampler.Dispose();
                }
                

                //A poor attempt at local audio recording.
                //Maybe I'll revisit it someday.
                /*
                if (msg.Message.StartsWith("!t2"))
                {
                    var foundChannel = msg.OriginalMessage.Server.VoiceChannels.First(d => d.Name == "Bot Test");
                    await client.JoinVoiceServer(foundChannel);
                    var voiceClient = client.GetVoiceClient(msg.OriginalMessage.Server);

                    var waveIn = new WasapiLoopbackCapture();
                    waveIn.ShareMode = AudioClientShareMode.Shared;

                    var memoryStream = new MemoryStream();
                    var sourceStream = new Wave32To16Stream(new RawSourceWaveStream(memoryStream, waveIn.WaveFormat));
                    var convertStream = new WaveFormatConversionStream(new WaveFormat(48000, 16, 1), sourceStream);

                    waveIn.DataAvailable += async delegate(object sender, WaveInEventArgs args)
                    {
                        await memoryStream.FlushAsync();
                        await memoryStream.WriteAsync(args.Buffer, 0, args.BytesRecorded);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        byte[] pcmBuffer = new byte[4096];

                        await convertStream.ReadAsync(pcmBuffer, 0, pcmBuffer.Length);
                        voiceClient.SendVoicePCM(pcmBuffer, pcmBuffer.Length);
                        await convertStream.FlushAsync();
                    };

                    waveIn.StartRecording();

                    await Task.Delay(100000);

                    waveIn.StopRecording();

                    convertStream.Dispose();
                    sourceStream.Dispose();
                    memoryStream.Dispose();

                    voiceClient.ClearVoicePCM();
                    await voiceClient.WaitVoice();
                }
                */

                if (msg.Message.StartsWith("!goodshit"))
                {
                    if (msg.Message.Length < 12)
                    {
                        return "Syntax is: !goodshit [first] [second]";
                    }

                    string[] parts = msg.Message.Substring(10).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 2)
                    {
                        return "Syntax is: !goodshit [first] [second] [emoji] [emoji]";
                    }

                    if (parts.Length < 4)
                    {
                        return new Copypasta(parts[0] + " " + parts[1], "👌", "👀").TotalCopypasta;
                    }

                    return new Copypasta(parts[0] + " " + parts[1], parts[2], parts[3]).TotalCopypasta;
                }

                if (msg.Message.StartsWith("!topmemers"))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine();
                                        
                    int iterator = 1;
                    foreach (var result in DatabaseContainer.Current.GetMessages().GroupBy(d => d.Username).OrderByDescending(d=>d.Count()))
                    {
                        builder.Append(iterator);
                        builder.Append(". ");
                        builder.Append(result.Key);
                        builder.Append($" ({result.Count()})");
                        builder.AppendLine();

                        iterator++;
                    }

                    return builder.ToString();
                }

                if (msg.Message.StartsWith("!mph"))
                {
                    int memecount = DatabaseContainer.Current.GetMessages().Count();
                    double duration = (DatabaseContainer.Current.GetMessages().Last().Timestamp - DatabaseContainer.Current.GetMessages().First().Timestamp).TotalHours;

                    return "Average Total Memes per Hour: " + (memecount/duration);
                }

                return null;
            });
        }
    }
}
