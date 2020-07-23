import numpy as np
import pandas as pd


def to_remove():
    rem = []
    for i in e_i:
        if set(R[i]).intersection(set(A[i])) == set(R[i]):
            rem.append(i)
    return rem


d = pd.read_csv('matrix.csv', header=None).to_numpy()

m = d + np.identity(len(d))
print('\nM:\n', m)

m_k1 = np.matrix(m, dtype=bool)
m_k2 = np.dot(m_k1, m_k1)
pwr_cnt = 2
while not (m_k1 == m_k2).all():
    m_k1 = m_k2
    m_k2 = np.dot(m_k1, np.matrix(m, dtype=bool))
    pwr_cnt += 1
new_M = np.matrix(m_k2, dtype=float)
print('\nM^{}:\n'.format(pwr_cnt), new_M)

R = np.array([np.nonzero(col)[1] for col in new_M.T])
A = np.array([np.nonzero(row)[1] for row in new_M])
print('\nR (input) (cols):\n', list(R + 1))
print('A (output) (rows):\n', list(A + 1))

e_i = list(range(len(new_M)))
answer = []
while len(e_i) > 0:
    trs = to_remove()
    answer.append(np.array(trs) + 1)
    e_i = [x for x in e_i if x not in trs]
    for tr in trs:
        R = [np.delete(row, np.argwhere(row == tr)) for row in R]
        A = [np.delete(row, np.argwhere(row == tr)) for row in A]

print('\nAnswer (First -> Last):', answer)
input("\npress close to exit")
