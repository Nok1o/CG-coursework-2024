import matplotlib.pyplot as plt

def get_avgs_from_file(filename):
    try:
        f = open(filename, "r+")
    except FileNotFoundError:
        print(f"Error: File '{filename}' not found.")
        exit()
    except Exception as e:
        print(f"Error reading CSV file: {e}")
        exit(1)

    # Print the first few rows to inspect the data structure
    print("CSV File Loaded Successfully!")

    print(line:=f.readline().strip().split(","))
    if line != ['Threads', 'Run', 'Time (ms)']:
        print("Unexpected columns!")
        exit(1)

    workers = []
    sums = []
    cur_num_workers = 0
    while line := f.readline():
        line = line.split(',')
        if int(line[0]) != cur_num_workers:
            cur_num_workers = int(line[0])
            workers.append(cur_num_workers)
            sums.append([0, 0])
        sums[-1][0] += float(line[-1])
        sums[-1][1] += 1

    return workers, [sm[0] / sm[1] for sm in sums]

avgs_knight = get_avgs_from_file('./bin/x64/Debug/render_times.csv')
avgs_spheres = get_avgs_from_file('render_times_knight_test.csv')


plt.figure(figsize=(8, 5))
plt.plot(avgs_knight[0], avgs_knight[1], color='skyblue', linestyle='--', marker='v')
#plt.plot(avgs_spheres[0], avgs_spheres[1], color='brown', linestyle='-.', marker='p')
plt.legend(['Сцена с конем', "Сцена со сферами"])
plt.xlabel("Число лучей, используемых для эффекта глубины поля")
plt.ylabel("Average Render Time (s)")
plt.title("Average Render Time per Scene")
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig("render_times_plot.pdf")
plt.show()