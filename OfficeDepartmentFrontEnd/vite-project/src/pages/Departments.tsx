import { useEffect, useState } from 'react';
import { Layout } from '../components/Layout';
import { departmentService } from '../api/departmentService';
import { branchOfficeService } from '../api/branchOfficeService';
import type { Department, BranchOffice } from '../types';
import { useAuth } from '../context/AuthContext';

export const Departments = () => {
  const [departments, setDepartments] = useState<Department[]>([]);
  const [branchOffices, setBranchOffices] = useState<BranchOffice[]>([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingDepartment, setEditingDepartment] = useState<Department | null>(null);
  const { isAdmin } = useAuth();

  const [formData, setFormData] = useState({
    name: '',
    description: '',
    branchOfficeId: '',
    managerId: '',
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [departmentsData, branchOfficesData] = await Promise.all([
        departmentService.getAll(),
        branchOfficeService.getAll(),
      ]);
      setDepartments(departmentsData);
      setBranchOffices(branchOfficesData);
    } catch (error) {
      console.error('Ошибка загрузки данных:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingDepartment) {
        await departmentService.update(editingDepartment.id, {
          ...formData,
          managerId: formData.managerId || undefined,
        });
      } else {
        await departmentService.create({
          ...formData,
          managerId: formData.managerId || undefined,
        });
      }
      setShowModal(false);
      setEditingDepartment(null);
      resetForm();
      loadData();
    } catch (error) {
      console.error('Ошибка при сохранении отдела:', error);
    }
  };

  const handleEdit = (department: Department) => {
    setEditingDepartment(department);
    setFormData({
      name: department.name,
      description: department.description,
      branchOfficeId: department.branchOfficeId,
      managerId: department.managerId || '',
    });
    setShowModal(true);
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Вы уверены, что хотите удалить этот отдел?')) return;
    try {
      await departmentService.delete(id);
      loadData();
    } catch (error) {
      console.error('Ошибка при удалении отдела:', error);
    }
  };

  const resetForm = () => {
    setFormData({
      name: '',
      description: '',
      branchOfficeId: '',
      managerId: '',
    });
  };

  if (loading) {
    return (
      <Layout>
        <div className="text-center py-12">Загрузка...</div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="px-4 py-6 sm:px-0">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-3xl font-bold text-gray-900">Отделы</h1>
          {isAdmin && (
            <button
              onClick={() => {
                setEditingDepartment(null);
                resetForm();
                setShowModal(true);
              }}
              className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-md text-sm font-medium"
            >
              Добавить отдел
            </button>
          )}
        </div>

        <div className="bg-white shadow overflow-hidden sm:rounded-md">
          <ul className="divide-y divide-gray-200">
            {departments.map((department) => (
              <li key={department.id}>
                <div className="px-4 py-4 sm:px-6">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center">
                      <div className="flex-shrink-0">
                        <div className="h-10 w-10 rounded-full bg-purple-100 flex items-center justify-center">
                          <span className="text-purple-600 font-medium">
                            {department.name.charAt(0).toUpperCase()}
                          </span>
                        </div>
                      </div>
                      <div className="ml-4">
                        <div className="text-sm font-medium text-gray-900">{department.name}</div>
                        <div className="text-sm text-gray-500">{department.description}</div>
                        {department.branchOffice && (
                          <div className="text-xs text-gray-400">
                            Офис: {department.branchOffice.name}
                          </div>
                        )}
                      </div>
                    </div>
                    {isAdmin && (
                      <div className="flex space-x-2">
                        <button
                          onClick={() => handleEdit(department)}
                          className="text-indigo-600 hover:text-indigo-900 text-sm font-medium"
                        >
                          Редактировать
                        </button>
                        <button
                          onClick={() => handleDelete(department.id)}
                          className="text-red-600 hover:text-red-900 text-sm font-medium"
                        >
                          Удалить
                        </button>
                      </div>
                    )}
                  </div>
                </div>
              </li>
            ))}
          </ul>
        </div>

        {showModal && (
          <div className="fixed z-10 inset-0 overflow-y-auto">
            <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
              <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" onClick={() => setShowModal(false)}></div>
              <div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
                <form onSubmit={handleSubmit} className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                  <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
                    {editingDepartment ? 'Редактировать отдел' : 'Добавить отдел'}
                  </h3>
                  <div className="space-y-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Название</label>
                      <input
                        type="text"
                        required
                        className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                        value={formData.name}
                        onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Описание</label>
                      <textarea
                        required
                        rows={3}
                        className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                        value={formData.description}
                        onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Офис</label>
                      <select
                        required
                        className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                        value={formData.branchOfficeId}
                        onChange={(e) => setFormData({ ...formData, branchOfficeId: e.target.value })}
                      >
                        <option value="">Выберите офис</option>
                        {branchOffices.map((bo) => (
                          <option key={bo.id} value={bo.id}>
                            {bo.name}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                  <div className="mt-5 sm:mt-6 sm:grid sm:grid-cols-2 sm:gap-3 sm:grid-flow-row-dense">
                    <button
                      type="submit"
                      className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:col-start-2 sm:text-sm"
                    >
                      {editingDepartment ? 'Обновить' : 'Создать'}
                    </button>
                    <button
                      type="button"
                      onClick={() => {
                        setShowModal(false);
                        setEditingDepartment(null);
                        resetForm();
                      }}
                      className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:col-start-1 sm:text-sm"
                    >
                      Отмена
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        )}
      </div>
    </Layout>
  );
};


