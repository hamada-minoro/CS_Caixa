using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;

namespace CS_Caixa
{
    public static class ClassFalarTexto
    {

        public static void FalarTexto(string texto, List<VoiceInfo> listaVozes, string voz)
        {
            VoiceInfo infoVoz;
            
            try
            {                
                SpeechSynthesizer vozSenha = new SpeechSynthesizer();
                infoVoz = listaVozes.Where(p => p.Name.Contains(voz)).FirstOrDefault();
                vozSenha.SelectVoice(infoVoz.Name);

                vozSenha.Speak(texto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<VoiceInfo> CarregaComboVozes()
        {
            List<VoiceInfo> listaVozes = new List<VoiceInfo>();
            VoiceInfo infoVoz;

            SpeechSynthesizer synth = new SpeechSynthesizer();
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                infoVoz = voice.VoiceInfo;
                listaVozes.Add(infoVoz);
            }

            return listaVozes;
        }
    }
}
