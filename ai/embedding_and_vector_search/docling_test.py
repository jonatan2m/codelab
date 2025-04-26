from pathlib import Path
from docling.document_converter import DocumentConverter

#source = "https://arxiv.org/pdf/2408.09869"  # document per local path or URL
source = "./arquivos/livro.pdf"
converter = DocumentConverter()
result = converter.convert(source)
#print(result.document.export_to_markdown())  # output: "## Docling Technical Report[...]"


# Export Docling document format to markdown:
output_dir = Path("./")
doc_filename = "livro"
with (output_dir / f"{doc_filename}.md").open("w") as fp:
    fp.write(result.document.export_to_markdown())