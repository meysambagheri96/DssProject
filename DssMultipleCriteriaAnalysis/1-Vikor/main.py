import numpy as np
import pandas as pd

a = pd.read_csv('matrix.csv', header=None).to_numpy()
w = pd.read_csv('weights.csv', header=None).to_numpy()

a2 = a**2
a2 = np.sqrt(a2.sum(axis=0))
print('\nEuclideans: ', np.round(a2, 4))

normd_a = a / a2
print('\nNormalized A:\n', np.round(normd_a, 4))

wd = normd_a * w
print('\nWeghted A:\n', np.round(wd, 4))

maxs = wd.max(axis=0)
mins = wd.min(axis=0)
dist = maxs - mins
print('\nMax each col:', np.round(maxs, 4))
print('Min each col:', np.round(mins, 4))
print('Max - Min:', np.round(dist, 4))

sk = (w * (maxs - wd) / dist).sum(axis=1)
rk = (w * (maxs - wd) / dist).max(axis=1)
print('\nS_k:', np.round(sk, 4))
print('R_k:', np.round(rk, 4))

lensk = max(sk) - min(sk)
lenrk = max(rk) - min(rk)

qk = 0.5 * (((sk - min(sk)) / lensk) + ((rk - min(rk)) / lenrk))
print('Q_k:', np.round(qk, 4))

sk_s = np.argsort(sk)
rk_s = np.argsort(rk)
qk_s = np.argsort(qk)
print('\nQ_k (Low -> High):', qk_s + 1)
print('R_k (Low -> High):', rk_s + 1)
print('S_k (Low -> High):', sk_s + 1)

input("\npress close to exit")
