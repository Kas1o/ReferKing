import os
import chardet

def get_encoding(file_path):
    """
    自动检测文件编码
    """
    with open(file_path, 'rb') as f:
        result = chardet.detect(f.read())
        return result['encoding']

def read_and_encode(file_path):
    """
    读取文件内容,并转换为UTF-8编码
    """
    encoding = get_encoding(file_path)
    with open(file_path, 'r', encoding=encoding) as f:
        content = f.read()
    return content.encode('utf-8').decode('utf-8')

if __name__ == '__main__':
    book_dir = './books/'
    for filename in os.listdir(book_dir):
        file_path = os.path.join(book_dir, filename)
        if os.path.isfile(file_path):
            try:
                content = read_and_encode(file_path)
                # 在此处理转码后的文件内容
                print(f'文件: {filename}\n内容: {content}\n')
            except Exception as e:
                print(f'无法处理文件 {filename}: {e}')