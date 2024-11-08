import zipfile
import os
import shutil
from tg_business_bot.Config import database_dir


def extract_and_save_images(file_path, image_tokens):
    output_folder = database_dir

    temp_folder = 'temp_extracted_files'
    if not os.path.exists(temp_folder):
        os.makedirs(temp_folder)

    with zipfile.ZipFile(file_path, 'r') as z:
        z.extractall(temp_folder)

        image_count = 0
        for root, dirs, files in os.walk(temp_folder):
            for file in files:
                if file.endswith(('.png', '.jpg', '.jpeg', '.gif', '.bmp')):
                    if image_count >= len(image_tokens):
                        raise ValueError("Количество найденных изображений превышает количество предоставленных имен.")

                    src_path = os.path.join(root, file)
                    file_extension = os.path.splitext(file)[1]
                    new_file_name = f"{image_tokens[image_count]}{file_extension}"
                    dst_path = os.path.join(output_folder, new_file_name)
                    shutil.move(src_path, dst_path)
                    print(f"Изображение сохранено: {dst_path}")
                    image_count += 1

    shutil.rmtree(temp_folder)
