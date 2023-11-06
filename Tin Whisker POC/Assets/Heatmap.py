import sys
import numpy as np
import pandas as pd
import seaborn as sns
import matplotlib.pyplot as plt
from collections import Counter

def create_heatmap(file_path):
    # Read the file
    with open(file_path, 'r') as file:
        lines = file.readlines()

    # Parse component pairs from the lines
    pairs = [tuple(line.strip().strip('()').split(',')) for line in lines[1:] if line.strip()]
    
    # Count the frequency of each unique component pair
    pair_counts = Counter(pairs)
    
    # Identify unique components
    unique_components = sorted(set(sum(pairs, ())))
    
    # Create a mapping from component to index
    component_to_index = {component: index for index, component in enumerate(unique_components)}
    
    # Initialize the frequency matrix
    frequency_matrix = np.zeros((len(unique_components), len(unique_components)), dtype=int)
    
    # Populate the matrix with the frequency counts
    for pair, count in pair_counts.items():
        i, j = component_to_index[pair[0]], component_to_index[pair[1]]
        frequency_matrix[i][j] = count
    
    # Convert the matrix to a DataFrame for the heatmap
    frequency_df = pd.DataFrame(frequency_matrix, index=unique_components, columns=unique_components)
    
    # Create the heatmap
    plt.figure(figsize=(20, 15))
    ax = sns.heatmap(frequency_df, annot=True, fmt="d", cmap="YlGnBu")
    ax.set_title('Heatmap of Bridged Component Pairs')
    plt.show()

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python script.py <file_path>")
        sys.exit(1)
    
    file_path = sys.argv[1]
    create_heatmap(file_path)
