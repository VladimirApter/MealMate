import uuid
import zipfile
import os
import shutil
import re
from PIL import Image
from tg_business_bot.Config import menu_item_images_dir


def compress_image(input_path, output_path, quality):
    with Image.open(input_path) as img:
        img.save(output_path, optimize=True, quality=quality)


def determine_compression_quality(file_size):
    if file_size < 50 * 1024:
        return 95
    elif file_size < 100 * 1024:
        return 85
    elif file_size < 200 * 1024:
        return 75
    elif file_size < 500 * 1024:
        return 65
    else:
        return 55


def extract_and_save_images(file_path):
    output_folder = menu_item_images_dir
    temp_folder = 'temp_extracted_files'
    image_paths = []

    if not os.path.exists(temp_folder):
        os.makedirs(temp_folder)
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    with zipfile.ZipFile(file_path, 'r') as z:
        file_list = z.infolist()

        image_files = [file_info for file_info in file_list if file_info.filename.endswith(('.png', '.jpg', '.jpeg', '.gif', '.bmp', '.webp'))]

        image_files.sort(key=lambda x: int(re.search(r'image(\d+)', x.filename).group(1)))

        for file_info in image_files:
            z.extract(file_info, temp_folder)
            src_path = os.path.join(temp_folder, file_info.filename)
            file_extension = os.path.splitext(file_info.filename)[1]
            unique_filename = f"{uuid.uuid4()}{file_extension}"
            dst_path = os.path.join(output_folder, unique_filename)

            file_size = os.path.getsize(src_path)
            quality = determine_compression_quality(file_size)
            compress_image(src_path, dst_path, quality)

            image_paths.append(unique_filename)

    shutil.rmtree(temp_folder)
    return image_paths
