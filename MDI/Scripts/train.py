#!/usr/bin/env python3
"""
YOLOv8 training script for MDI Object Detection.
Usage: python train.py --dataset <dataset_dir> --output <output_dir>
Requires: pip install ultralytics
"""
import argparse
import os
import random
import shutil
import sys


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--dataset', required=True)
    parser.add_argument('--output',  required=True)
    parser.add_argument('--name',    default='model')
    parser.add_argument('--epochs',  type=int, default=50)
    parser.add_argument('--imgsz',   type=int, default=640)
    args = parser.parse_args()

    dataset_dir  = args.dataset
    output_dir   = args.output
    images_dir   = os.path.join(dataset_dir, 'images')
    labels_dir   = os.path.join(dataset_dir, 'labels')
    classes_file = os.path.join(dataset_dir, 'classes.txt')

    # ── Validate inputs ──────────────────────────────────────────────────────────
    if not os.path.exists(classes_file):
        print(f'ERROR: classes.txt not found at {classes_file}', flush=True)
        sys.exit(1)

    with open(classes_file) as f:
        classes = [l.strip() for l in f if l.strip()]

    if not classes:
        print('ERROR: classes.txt is empty.', flush=True)
        sys.exit(1)

    supported = ('.jpg', '.jpeg', '.png')
    all_images = [f for f in os.listdir(images_dir)
                  if f.lower().endswith(supported)] if os.path.isdir(images_dir) else []

    if not all_images:
        print('ERROR: No images found in dataset/images/.', flush=True)
        sys.exit(1)

    # Warn about images with missing or empty label files
    missing = []
    for img in all_images:
        base = os.path.splitext(img)[0]
        lbl  = os.path.join(labels_dir, base + '.txt')
        if not os.path.exists(lbl) or os.path.getsize(lbl) == 0:
            missing.append(img)
    if missing:
        print(f'WARNING: {len(missing)} image(s) have no labels — they will count as background.', flush=True)
        for m in missing[:5]:
            print(f'  {m}', flush=True)

    print(f'Dataset  : {dataset_dir}', flush=True)
    print(f'Images   : {len(all_images)} total  ({len(missing)} unlabeled)', flush=True)
    print(f'Classes  : {", ".join(classes)}', flush=True)
    print(f'Epochs   : {args.epochs}', flush=True)

    # ── 80/20 train/val split ────────────────────────────────────────────────────
    random.shuffle(all_images)
    split      = max(1, int(len(all_images) * 0.8))
    train_imgs = all_images[:split]
    val_imgs   = all_images[split:] if len(all_images) > 1 else all_images[:1]

    print(f'Split    : {len(train_imgs)} train  /  {len(val_imgs)} val', flush=True)

    train_txt = os.path.join(dataset_dir, 'train.txt')
    val_txt   = os.path.join(dataset_dir, 'val.txt')

    with open(train_txt, 'w') as f:
        for img in train_imgs:
            f.write(os.path.join(images_dir, img) + '\n')

    with open(val_txt, 'w') as f:
        for img in val_imgs:
            f.write(os.path.join(images_dir, img) + '\n')

    # ── data.yaml ────────────────────────────────────────────────────────────────
    yaml_path = os.path.join(dataset_dir, 'data.yaml')
    with open(yaml_path, 'w') as f:
        f.write(f'path: {dataset_dir}\n')
        f.write(f'train: {train_txt}\n')
        f.write(f'val: {val_txt}\n')
        f.write(f'nc: {len(classes)}\n')
        f.write(f'names: {classes}\n')

    # ── Train ────────────────────────────────────────────────────────────────────
    try:
        from ultralytics import YOLO
    except ImportError:
        print('ERROR: ultralytics not installed.', flush=True)
        print('Fix  : pip install ultralytics', flush=True)
        sys.exit(1)

    os.makedirs(output_dir, exist_ok=True)

    print('Starting training...', flush=True)
    model = YOLO('yolov8n.pt')
    model.train(
        data=yaml_path,
        epochs=args.epochs,
        imgsz=args.imgsz,
        project=output_dir,
        name='run',
        exist_ok=True,
        verbose=True,
    )

    # ── Export to ONNX ───────────────────────────────────────────────────────────
    best_pt = os.path.join(output_dir, 'run', 'weights', 'best.pt')
    if not os.path.exists(best_pt):
        print(f'ERROR: best.pt not found at {best_pt}', flush=True)
        sys.exit(1)

    print('Exporting to ONNX (opset 17)...', flush=True)
    export_model = YOLO(best_pt)
    # opset=17 — compatible with OnnxRuntime 1.16+; simplify omitted (requires onnxsim)
    export_model.export(format='onnx', imgsz=args.imgsz, opset=17)

    onnx_src  = best_pt.replace('.pt', '.onnx')
    onnx_dest = os.path.join(output_dir, args.name + '.onnx')
    if os.path.exists(onnx_src):
        shutil.copy(onnx_src, onnx_dest)
        print(f'Model saved: {onnx_dest}', flush=True)
    else:
        print(f'ERROR: ONNX export not found at {onnx_src}', flush=True)
        sys.exit(1)

    print('Training complete!', flush=True)


if __name__ == '__main__':
    main()
