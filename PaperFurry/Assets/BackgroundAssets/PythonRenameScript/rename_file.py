import os

directory = 'D:\\Users\\恩凡\\PS\\PaperFurry\\OriginalPhoto'
prefix = 'A-PaperFurryBg'
start_number = 1

for file_name in os.listdir(directory):
    new_name = f"{prefix}{str(start_number).zfill(3)}{os.path.splitext(file_name)[1]}"
    old_file_path = os.path.join(directory, file_name)
    new_file_path = os.path.join(directory, new_name)
    os.rename(old_file_path, new_file_path)
    start_number += 1