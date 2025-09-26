from PIL import Image
import pyzbar.pyzbar as pyzbar
import requests

def read_qr_code(image_path):
    # Ler a imagem do QR code
    image = Image.open(image_path)
    
    # Decodificar o QR code
    decoded_objects = pyzbar.decode(image)
    
    for obj in decoded_objects:
        # Extrair os dados do QR code
        data = obj.data.decode('utf-8')
        
        try:
            # Fazer uma requisição GET para a URL encontrada no QR code
            response = requests.get(data)
            response.raise_for_status()  # Verificar se a requisição foi bem sucedida
            
            return data
        except requests.exceptions.RequestException as e:
            print(f"Erro ao buscar URL: {e}")
    
    return None