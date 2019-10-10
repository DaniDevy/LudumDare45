using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
	public class AudioManager : MonoBehaviour {

		public Sound[] sounds;

		public bool muted;

		public bool muteMusic;
		
		public static AudioManager Instance { get; set; }
       
		void Awake () {
			Instance = this;
 
			foreach (Sound s in sounds) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
 
				s.source.loop = s.loop;
				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.bypassListenerEffects = s.bypass;
			}

			Play("Song");
			SetVolume(0.7f);
		}

		public void MuteSounds() {
			muted = !muted;
		}
		
		public void MuteMusic() {
			muteMusic = !muteMusic;
			float v;
			if (muteMusic) v = 0f;
			else v = 1f;
			foreach (Sound s in sounds) {
				if (s.name == "Song") {
					s.source.volume = v;
					return;
				}
			}
		}

		public void SetVolume(float v) {
			foreach (Sound s in sounds) {
				if (s.name == "Song") {
					s.source.volume = v;
					return;
				}
			}
		}

		public void UnmuteMusic() {
			foreach (Sound s in sounds) {
				if (s.name == "Song") {
					s.source.volume = 1.15f;
					return;
				}
			}
		}
 
		public void Play(string n) {
			if (muted && n != "Song") return;
			foreach (Sound s in sounds) {
				if (s.name == n) {
					s.source.Play();
					return;
				}
			}
		}

		public void Stop(string n) {
			foreach (Sound s in sounds) {
				if (s.name == n) {
					s.source.Stop();
					return;
				}
			}
		}
	}
}

