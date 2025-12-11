import { Layout } from '../components/Layout';
import { Link } from 'react-router-dom';

export const Dashboard = () => {
  return (
    <Layout>
      <div className="px-4 py-6 sm:px-0">
        <h1 className="text-3xl font-bold text-gray-900 mb-6">Панель управления</h1>
        
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
          <Link
            to="/head-offices"
            className="bg-white overflow-hidden shadow rounded-lg hover:shadow-lg transition-shadow"
          >
            <div className="p-5">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="w-8 h-8 bg-indigo-500 rounded-md flex items-center justify-center">
                    <span className="text-white text-xl">🏢</span>
                  </div>
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Главные офисы</dt>
                    <dd className="text-lg font-medium text-gray-900">Управление главными офисами</dd>
                  </dl>
                </div>
              </div>
            </div>
          </Link>

          <Link
            to="/branch-offices"
            className="bg-white overflow-hidden shadow rounded-lg hover:shadow-lg transition-shadow"
          >
            <div className="p-5">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="w-8 h-8 bg-green-500 rounded-md flex items-center justify-center">
                    <span className="text-white text-xl">🏛️</span>
                  </div>
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Дочерние офисы</dt>
                    <dd className="text-lg font-medium text-gray-900">Управление дочерними офисами</dd>
                  </dl>
                </div>
              </div>
            </div>
          </Link>

          <Link
            to="/employees"
            className="bg-white overflow-hidden shadow rounded-lg hover:shadow-lg transition-shadow"
          >
            <div className="p-5">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="w-8 h-8 bg-blue-500 rounded-md flex items-center justify-center">
                    <span className="text-white text-xl">👥</span>
                  </div>
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Сотрудники</dt>
                    <dd className="text-lg font-medium text-gray-900">Управление сотрудниками</dd>
                  </dl>
                </div>
              </div>
            </div>
          </Link>

          <Link
            to="/departments"
            className="bg-white overflow-hidden shadow rounded-lg hover:shadow-lg transition-shadow"
          >
            <div className="p-5">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="w-8 h-8 bg-purple-500 rounded-md flex items-center justify-center">
                    <span className="text-white text-xl">📁</span>
                  </div>
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Отделы</dt>
                    <dd className="text-lg font-medium text-gray-900">Управление отделами</dd>
                  </dl>
                </div>
              </div>
            </div>
          </Link>

          <Link
            to="/tasks"
            className="bg-white overflow-hidden shadow rounded-lg hover:shadow-lg transition-shadow"
          >
            <div className="p-5">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <div className="w-8 h-8 bg-yellow-500 rounded-md flex items-center justify-center">
                    <span className="text-white text-xl">✓</span>
                  </div>
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Задачи</dt>
                    <dd className="text-lg font-medium text-gray-900">Управление задачами</dd>
                  </dl>
                </div>
              </div>
            </div>
          </Link>
        </div>
      </div>
    </Layout>
  );
};

