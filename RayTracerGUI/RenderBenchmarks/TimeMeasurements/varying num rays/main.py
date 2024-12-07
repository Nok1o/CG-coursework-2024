import pandas as pd
import matplotlib.pyplot as plt

def get_avgs_from_file(filename):
    try:
        df = pd.read_csv(filename)
    except FileNotFoundError:
        print(f"Error: File '{filename}' not found.")
        exit()
    except Exception as e:
        print(f"Error reading CSV file: {e}")
        exit()

    # Print the first few rows to inspect the data structure
    print("CSV File Loaded Successfully!")
    print(df.head())

    avgs = []
    curr_sc = df['Scene'][0]
    s = 0
    a = []
    for i in range(len(df['Run'])):
        if df['Scene'][i] == curr_sc:
            #s += df['Time (ms)'][i]
            a.append(df['Time (ms)'][i])
        else:
            #s /= df['Run'][i - 1]
            a.sort()
            #avgs.append(s / 1000.0)
            avgs.append(a[len(a) // 2])
            a.clear()
            s = 0
            curr_sc = df['Scene'][i]
    #s /= df['Run'][len(df['Run']) - 1]
    #avgs.append(s / 1000.0)

    a.sort()
    # avgs.append(s / 1000.0)
    avgs.append(a[len(a) // 2])
    a.clear()
    return avgs

avgs_knight = get_avgs_from_file('render_times_knight.csv')
avgs_spheres = get_avgs_from_file('render_times_spheres.csv')


plt.figure(figsize=(8, 5))
plt.plot([2 ** i for i in range(len(avgs_knight))], avgs_knight, color='skyblue', linestyle='--', marker='v')
plt.plot([10 * i if i > 0 else 1 for i in range(0, len(avgs_spheres))], avgs_spheres, color='brown', linestyle='-.', marker='p')
plt.legend(['Сцена с конем', "Сцена со сферами"])
plt.xlabel("Число лучей, используемых для эффекта глубины поля")
plt.ylabel("Average Render Time (s)")
plt.title("Average Render Time per Scene")
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig("render_times_plot.pdf")
plt.show()