import os
import numpy as np
import pandas as pd
import seaborn as sns
import matplotlib.pyplot as plt
from collections import Counter

def create_heatmap(directory_path):
    total_pair_counts = Counter()
    file_count = 0  # To keep track of the number of files processed

    for file_name in os.listdir(directory_path):
        if file_name.startswith("sim_") and file_name.endswith("_bridged_components.txt"):
            file_count += 1
            file_path = os.path.join(directory_path, file_name)
            
            with open(file_path, 'r') as file:
                lines = file.readlines()
                pairs = [tuple(line.strip().strip('()').split(',')) for line in lines[1:] if line.strip()]
            
            total_pair_counts.update(Counter(pairs))

    if file_count == 0:
        print("No valid files found in the directory.")
        return

    unique_components = sorted(set(sum(total_pair_counts.keys(), ())))
    component_to_index = {component: index for index, component in enumerate(unique_components)}
    frequency_matrix = np.zeros((len(unique_components), len(unique_components)), dtype=float)

    for pair, count in total_pair_counts.items():
        i, j = component_to_index[pair[0]], component_to_index[pair[1]]
        frequency_matrix[i][j] = (count / file_count) * 100

    frequency_df = pd.DataFrame(frequency_matrix, index=unique_components, columns=unique_components)
    plt.figure(figsize=(20, 15))
    
    # Only annotate cells with values greater than 0
    annotations = np.where(frequency_matrix > 0, frequency_matrix.round(1).astype(str), np.full(frequency_matrix.shape, ""))
    
    ax = sns.heatmap(frequency_df, annot=annotations, fmt="s", cmap="YlGnBu", vmin=0, vmax=100, linewidths=0.5, annot_kws={"fontsize": 8})
    ax.set_title('Percentage Heatmap of Bridged Component Pairs Across Multiple Files')

    # Save the heatmap as an image file in the same directory as the provided directory path
    output_file = os.path.join(directory_path, "heatmap_image.png")
    plt.savefig(output_file)

    plt.close()  # Close the plot to free up memory

if __name__ == "__main__":
    directory_path = "Assets/BridgedComponentsResults"
    if os.path.isdir(directory_path):
        create_heatmap(directory_path)
    else:
        print(f"Directory {directory_path} does not exist")
