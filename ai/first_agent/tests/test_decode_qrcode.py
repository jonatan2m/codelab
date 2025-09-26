import unittest
from tools.decode_qrcode import read_qr_code  # Ajuste conforme necessário

class TestDecodeQrCode(unittest.TestCase):
    def test_read_qr_code(self):
        image_path = 'tests/test_image.jpg'  # Certifique-se de que o caminho está correto
        url = read_qr_code(image_path)
        self.assertIsNotNone(url)
        self.assertTrue(url.startswith('http://') or url.startswith('https://'))

if __name__ == '__main__':
    unittest.main()

