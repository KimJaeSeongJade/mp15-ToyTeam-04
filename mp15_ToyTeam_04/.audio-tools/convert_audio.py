import hashlib
import json
import pathlib
import subprocess
import sys
import tempfile
import wave
import zipfile

sys.path.insert(0, str(pathlib.Path(__file__).parent))
import imageio_ffmpeg

root = pathlib.Path(__file__).resolve().parent.parent
destination = root / 'output' / 'Audio_WAV'
destination.mkdir(parents=True, exist_ok=True)
encoder = imageio_ffmpeg.get_ffmpeg_exe()
records = []
with zipfile.ZipFile(r'C:\Users\user\Downloads\Audio.zip') as archive:
    for entry in archive.infolist():
        if entry.is_dir():
            continue
        name = pathlib.PurePosixPath(entry.filename).name
        suffix = pathlib.Path(name).suffix.lower()
        if suffix not in {'.wav', '.mp3', '.ogg', '.flac'}:
            raise RuntimeError(f'Unexpected file: {name}')
        data = archive.read(entry)
        target = destination / (pathlib.Path(name).stem + '.wav')
        if target.exists():
            raise RuntimeError(f'Output exists: {target}')
        if suffix == '.wav':
            target.write_bytes(data)
            assert hashlib.sha256(target.read_bytes()).digest() == hashlib.sha256(data).digest()
        else:
            with tempfile.TemporaryDirectory() as temporary:
                source = pathlib.Path(temporary) / name
                source.write_bytes(data)
                subprocess.run([encoder, '-v', 'error', '-nostdin', '-i', str(source),
                                '-map', '0:a:0', '-c:a', 'pcm_s16le', str(target)], check=True)
        subprocess.run([encoder, '-v', 'error', '-nostdin', '-i', str(target),
                        '-f', 'null', '-'], check=True)
        record = {'source': name, 'output': target.name, 'unchanged': suffix == '.wav'}
        if suffix != '.wav':
            with wave.open(str(target), 'rb') as audio:
                assert audio.getsampwidth() == 2
                assert audio.getnframes() > 0
                record.update(channels=audio.getnchannels(), sample_rate=audio.getframerate(),
                              seconds=round(audio.getnframes() / audio.getframerate(), 3))
        records.append(record)
zip_path = root / 'output' / 'Audio_WAV.zip'
if zip_path.exists():
    raise RuntimeError('Output ZIP already exists')
with zipfile.ZipFile(zip_path, 'w', zipfile.ZIP_DEFLATED) as output_zip:
    for record in records:
        output_zip.write(destination / record['output'], 'Audio/' + record['output'])
(root / 'output' / 'Audio_WAV_conversion.json').write_text(json.dumps(records, indent=2), encoding='utf-8')
print(json.dumps({'total': len(records), 'converted': sum(not r['unchanged'] for r in records),
                  'preserved': sum(r['unchanged'] for r in records), 'zip': str(zip_path)}, indent=2))
