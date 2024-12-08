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

def get_median_from_file(filename):
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
    times = []
    cur_times = []
    cur_num_workers = 0
    while line := f.readline():
        line = line.strip().split(',')
        if int(line[0]) != cur_num_workers:
            if (cur_num_workers != 0):
                cur_times.sort()
                times.append(cur_times[len(cur_times) // 2])

            cur_num_workers = int(line[0])
            workers.append(cur_num_workers)
            cur_times.clear()
        cur_times.append(float(line[-1]))

    if (cur_num_workers != 0):
        cur_times.sort()
        times.append(cur_times[len(cur_times) // 2])

    return workers, times


#med_singlesphere = get_median_from_file('render_times_50_singlesphere_cracked.csv')
# med_singlesphere = get_median_from_file('render_times_50_doublesphere_1.csv')
# avgs_singlesphere = get_avgs_from_file('render_times_50_doublesphere_1.csv')

med_singlesphere = get_median_from_file('./bin/x64/Debug/render_times.csv')
avgs_singlesphere = get_avgs_from_file('./bin/x64/Debug/render_times.csv')

#avgs_spheres = get_avgs_from_file('render_times_knight_test.csv')


plt.figure(figsize=(8, 5))
plt.plot(avgs_singlesphere[0], avgs_singlesphere[1], color='skyblue', linestyle='--', marker='v')
plt.plot(med_singlesphere[0], med_singlesphere[1], color='brown', linestyle='-.', marker='p')
plt.legend(['Средние сферы', "Медианные сферы"])
plt.xlabel("Число лучей, используемых для эффекта глубины поля")
plt.ylabel("Average Render Time (s)")
plt.title("Average Render Time per Scene")
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig("render_times_plot.pdf")
plt.show()