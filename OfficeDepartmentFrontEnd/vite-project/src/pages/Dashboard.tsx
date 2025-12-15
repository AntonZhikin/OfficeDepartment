import { useEffect, useState } from 'react';
import { Layout } from '../components/Layout';
import { Link } from 'react-router-dom';
import { branchOfficeService } from '../api/branchOfficeService';
import { employeeService } from '../api/employeeService';
import { departmentService } from '../api/departmentService';
import { taskService } from '../api/taskService';
import { useAuth } from '../context/AuthContext';

export const Dashboard = () => {
  const [stats, setStats] = useState({
    offices: 0,
    employees: 0,
    departments: 0,
    tasks: 0,
  });
  const [loading, setLoading] = useState(true);
  const { user } = useAuth();

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      const [offices, employees, departments, tasks] = await Promise.all([
        branchOfficeService.getAll(),
        employeeService.getAll(),
        departmentService.getAll(),
        taskService.getAll(),
      ]);
      setStats({
        offices: offices.length,
        employees: employees.length,
        departments: departments.length,
        tasks: tasks.length,
      });
    } catch (error) {
      console.error('Ошибка загрузки статистики:', error);
    } finally {
      setLoading(false);
    }
  };

  const cards = [
    {
      title: 'Офисы',
      description: 'Управление офисами',
      count: stats.offices,
      link: '/branch-offices',
      gradient: 'from-emerald-500 to-teal-600',
      icon: '🏢',
      bgColor: 'bg-emerald-50',
    },
    {
      title: 'Сотрудники',
      description: 'Управление сотрудниками',
      count: stats.employees,
      link: '/employees',
      gradient: 'from-blue-500 to-indigo-600',
      icon: '👥',
      bgColor: 'bg-blue-50',
    },
    {
      title: 'Отделы',
      description: 'Управление отделами',
      count: stats.departments,
      link: '/departments',
      gradient: 'from-purple-500 to-pink-600',
      icon: '📁',
      bgColor: 'bg-purple-50',
    },
    {
      title: 'Задачи',
      description: 'Управление задачами',
      count: stats.tasks,
      link: '/tasks',
      gradient: 'from-amber-500 to-orange-600',
      icon: '✓',
      bgColor: 'bg-amber-50',
    },
  ];

  if (loading) {
    return (
      <Layout>
        <div className="px-4 py-6 sm:px-0">
          <div className="text-center py-12">Загрузка...</div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="px-4 py-6 sm:px-0">
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-gray-900 mb-2">
            Добро пожаловать, {user?.username}!
          </h1>
          <p className="text-lg text-gray-600">Панель управления системой</p>
        </div>

        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-2">
          {cards.map((card) => (
            <Link
              key={card.link}
              to={card.link}
              className="group relative overflow-hidden rounded-2xl bg-white shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-1"
            >
              <div className={`absolute inset-0 bg-gradient-to-br ${card.gradient} opacity-0 group-hover:opacity-10 transition-opacity duration-300`}></div>
              
              <div className="relative p-8">
                <div className="flex items-start justify-between mb-6">
                  <div className={`${card.bgColor} rounded-2xl p-4 transform group-hover:scale-110 transition-transform duration-300`}>
                    <span className="text-4xl">{card.icon}</span>
                  </div>
                  <div className="text-right">
                    <div className="text-4xl font-bold text-gray-900 mb-1">
                      {card.count}
                    </div>
                    <div className="text-sm text-gray-500">всего</div>
                  </div>
                </div>

                <div>
                  <h3 className="text-2xl font-bold text-gray-900 mb-2 group-hover:text-indigo-600 transition-colors">
                    {card.title}
                  </h3>
                  <p className="text-gray-600 text-sm">{card.description}</p>
                </div>

                <div className="mt-6 flex items-center text-indigo-600 font-medium group-hover:text-indigo-700">
                  <span className="text-sm">Перейти</span>
                  <svg
                    className="ml-2 w-5 h-5 transform group-hover:translate-x-1 transition-transform"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M9 5l7 7-7 7"
                    />
                  </svg>
                </div>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </Layout>
  );
};

