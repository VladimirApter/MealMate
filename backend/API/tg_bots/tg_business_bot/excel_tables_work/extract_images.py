import uuid
import zipfile
import os
import shutil
from tg_business_bot.Config import menu_item_images_dir


def extract_and_save_images(file_path):
    output_folder = menu_item_images_dir

    temp_folder = 'temp_extracted_files'
    image_paths = []

    if not os.path.exists(temp_folder):
        os.makedirs(temp_folder)

    with zipfile.ZipFile(file_path, 'r') as z:
        z.extractall(temp_folder)

        for root, dirs, files in os.walk(temp_folder):
            for file in files:
                if file.endswith(('.png', '.jpg', '.jpeg', '.gif', '.bmp', '.webp')):
                    src_path = os.path.join(root, file)
                    file_extension = os.path.splitext(file)[1]
                    unique_filename = f"{uuid.uuid4()}{file_extension}"
                    dst_path = os.path.join(output_folder, unique_filename)
                    shutil.move(src_path, dst_path)

                    image_paths.append(unique_filename)

    shutil.rmtree(temp_folder)
    return image_paths
